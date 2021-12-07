using System.Linq.Expressions;

namespace EduResourceAPI.Data.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task Delete(int entityId);
        Task<IList<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null!, string includeProperties = "");
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
    }
}