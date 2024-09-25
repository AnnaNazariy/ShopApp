using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using System.Reflection;
using System.Text;
using System.ComponentModel;
using Dapper_Example.DAL.Repositories.Interfaces;
using Dapper_Example.DAL.Entities;

namespace Dapper_Example.DAL.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected MySqlConnection _mySqlConnection;
        protected IDbTransaction _dbTransaction;
        private readonly string _tableName;

        protected GenericRepository(MySqlConnection mySqlConnection, IDbTransaction dbTransaction, string tableName)
        {
            _mySqlConnection = mySqlConnection;
            _dbTransaction = dbTransaction;
            _tableName = tableName;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _mySqlConnection.QueryAsync<T>($"SELECT * FROM {_tableName}", transaction: _dbTransaction);
        }

        public async Task<T> GetAsync(int id)
        {
            var result = await _mySqlConnection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {_tableName} WHERE Id=@Id",
                param: new { Id = id },
                transaction: _dbTransaction);
            if (result == null)
                throw new KeyNotFoundException($"{_tableName} with id [{id}] could not be found.");
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _mySqlConnection.ExecuteAsync($"DELETE FROM {_tableName} WHERE Id=@Id",
                param: new { Id = id },
                transaction: _dbTransaction);
        }

        public async Task<int> AddAsync(T entity)
        {
            var insertQuery = GenerateInsertQuery();
            var newId = await _mySqlConnection.ExecuteScalarAsync<int>(insertQuery,
                param: entity,
                transaction: _dbTransaction);
            return newId;
        }

        public async Task ReplaceAsync(T entity)
        {
            var updateQuery = GenerateUpdateQuery();
            await _mySqlConnection.ExecuteAsync(updateQuery,
                param: entity,
                transaction: _dbTransaction);
        }

        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();
        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }

        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
            var properties = GenerateListOfProperties(GetProperties);
            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });
            updateQuery.Remove(updateQuery.Length - 1, 1); // remove last comma
            updateQuery.Append(" WHERE Id=@Id");
            return updateQuery.ToString();
        }

        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {_tableName} ");
            insertQuery.Append("(");
            var properties = GenerateListOfProperties(GetProperties);
            properties.Remove("Id");
            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });
            insertQuery.Remove(insertQuery.Length - 1, 1).Append(") VALUES (");
            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });
            insertQuery.Remove(insertQuery.Length - 1, 1).Append(")");
            insertQuery.Append("; SELECT SCOPE_IDENTITY()");
            return insertQuery.ToString();
        }
    }
}
