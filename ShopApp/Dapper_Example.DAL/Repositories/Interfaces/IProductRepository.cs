using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper_Example.DAL.Entities;

namespace Dapper_Example.DAL.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> ProductByCategoryAsync(int categoryId);
    }
}
