using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EduResourceAPI.Data.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly EduResourceDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(EduResourceDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async virtual Task<IList<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null!,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.ToListAsync();
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbContext.Update(entityToUpdate);
        }

        public async virtual Task Delete(int entityId)
        {
            var entityToDelete = await _dbSet.FindAsync(entityId);
            if (entityToDelete is not null) _dbSet.Remove(entityToDelete);
        }
    }
}
