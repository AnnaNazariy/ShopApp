using Dapper;
using Dapper_Example.DAL.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Dapper_Example.DAL.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {

        public ProductRepository(SqlConnection sqlConnection, 
            IDbTransaction dbtransaction) : base(sqlConnection, dbtransaction, "Product")
        {
        }

        public async Task<IEnumerable<Product>> ProductByCategoryAsync (int productsCategoryId)
        {
            string sql = @"SELECT * FROM Product WHERE CategoryId = @ProductsCategoryId";

            var results = await _sqlConnection.QueryAsync<Product>(sql,
                param: new { ProductsCategoryId = productsCategoryId },
                transaction: _dbTransaction);
            return results;

        }
    }
}
