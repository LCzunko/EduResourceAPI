using EduResourceAPI.Data.Repositories;
using EduResourceAPI.Models.Entities;

namespace EduResourceAPI.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private EduResourceDbContext _dbContext;

        public UnitOfWork(EduResourceDbContext dbContext)
        {
            _dbContext = dbContext;
            Authors = new BaseRepository<Author>(dbContext);
            Categories = new BaseRepository<Category>(dbContext);
            Materials = new BaseRepository<Material>(dbContext);
            Reviews = new BaseRepository<Review>(dbContext);
        }

        public IBaseRepository<Author> Authors { get; }
        public IBaseRepository<Category> Categories { get; }
        public IBaseRepository<Material> Materials { get; }
        public IBaseRepository<Review> Reviews { get; }

        public async Task<bool> Commit()
        {
            return await _dbContext.SaveChangesAsync() != 0;
        }
    }
}
