using Dapper;
using Dapper_Example.DAL.Repositories.Interfaces;
using Dapper_Example_Project.Connection;
using System.Data;
using MySql.Data.MySqlClient;
using Dapper_Example.DAL.Entities;

namespace Dapper_Example.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(MySqlConnection mySqlConnection, IDbTransaction dbTransaction)
            : base(mySqlConnection, dbTransaction, "users")
        {
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            string sql = "SELECT * FROM Users WHERE Email = @Email";
            var user = await _mySqlConnection.QuerySingleOrDefaultAsync<User>(sql,
                new { Email = email },
                transaction: _dbTransaction);
            return user;
        }
    }
}
