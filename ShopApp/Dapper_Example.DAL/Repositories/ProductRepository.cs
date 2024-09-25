using Dapper;
using Dapper_Example.DAL.Repositories.Interfaces;
using Dapper_Example.DAL.Entities;
using System.Collections.Generic;
using Dapper_Example_Project.Connection;
using System.Data;
using MySql.Data.MySqlClient;

namespace Dapper_Example.DAL.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(MySqlConnection mySqlConnection, IDbTransaction dbTransaction)
            : base(mySqlConnection, dbTransaction, "products")
        {
        }

        public async Task<IEnumerable<Product>> ProductByCategoryAsync(int categoryId)
        {
            string sql = "SELECT * FROM Products WHERE CategoryId = @CategoryId";
            var results = await _mySqlConnection.QueryAsync<Product>(sql,
                new { CategoryId = categoryId },
                transaction: _dbTransaction);
            return results;
        }
    }
}
