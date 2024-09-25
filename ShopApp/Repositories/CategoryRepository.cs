using Dapper;
using Dapper_Example.DAL.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Dapper_Example.DAL.Repositories
{
    public class CategoryRepository : GenericRepository<Review>, ICategoryRepository
    {
        public CategoryRepository(SqlConnection sqlConnection, IDbTransaction dbtransaction) : base(sqlConnection, dbtransaction, "Category")
        {
        }

        public async Task<IEnumerable<Review>> TopFiveCategoryAsync()
        {
            string sql = @"SELECT TOP 5 * FROM Category";
            var results = await _sqlConnection.QueryAsync<Review>(sql,
                transaction: _dbTransaction);
            return results;
        }
    }
}
