using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace Dapper_Example_Project.Connection
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("MySqlConnection");
            var connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public void Dispose()
        {
            // Implement dispose pattern if necessary
        }
    }
}
