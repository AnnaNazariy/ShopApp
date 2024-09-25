using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper_Example.DAL.Entities;

namespace Dapper_Example.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<int> AddAsync(T entity);
        Task ReplaceAsync(T entity);
        Task DeleteAsync(int id);
    }
}
