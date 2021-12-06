using EduResourceAPI.Data.Repositories;
using EduResourceAPI.Models.Entities;

namespace EduResourceAPI.Data
{
    public interface IUnitOfWork
    {
        IBaseRepository<Author> Authors { get; }
        IBaseRepository<Category> Categories { get; }
        IBaseRepository<Material> Materials { get; }
        IBaseRepository<Review> Reviews { get; }

        Task<bool> Commit();
    }
}