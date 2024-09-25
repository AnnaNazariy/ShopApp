using Dapper_Example.DAL;

namespace Dapper_Example.DAL.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> TopFiveCategoryAsync();
    }
}
