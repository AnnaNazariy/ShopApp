using Dapper_Example.DAL.Entities;
using System.Threading.Tasks;

namespace Dapper_Example.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
    }
}
