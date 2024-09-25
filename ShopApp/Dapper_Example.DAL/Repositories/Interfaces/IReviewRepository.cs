using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper_Example.DAL.Entities;

namespace Dapper_Example.DAL.Repositories.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> ReviewsByProductAsync(int productId);
    }
}
