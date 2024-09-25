using Dapper;
using Dapper_Example.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using Dapper_Example.DAL.Entities;
using Dapper_Example_Project.Connection;
using System.Data;
using MySql.Data.MySqlClient;

namespace Dapper_Example.DAL.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(MySqlConnection mySqlConnection, IDbTransaction dbTransaction)
            : base(mySqlConnection, dbTransaction, "reviews")
        {
        }

        public async Task<IEnumerable<Review>> ReviewsByProductAsync(int productId)
        {
            string sql = "SELECT * FROM Reviews WHERE ProductId = @ProductId";
            var results = await _mySqlConnection.QueryAsync<Review>(sql,
                new { ProductId = productId },
                transaction: _dbTransaction);
            return results;
        }
    }
}
