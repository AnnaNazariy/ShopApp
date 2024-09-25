using System.Data;

namespace Dapper_Example.DAL.Repositories.Interfaces
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IProductRepository _productRepository { get; }

        public ICategoryRepository _categoryRepository { get; }

        readonly IDbTransaction _dbTransaction;

        public UnitOfWork(IProductRepository productRepository, 
            ICategoryRepository categoryRepository, 
            IDbTransaction dbTransaction)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _dbTransaction = dbTransaction;
        }

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
                // By adding this we can have muliple transactions as part of a single request
                //_dbTransaction.Connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                _dbTransaction.Rollback();
            }
        }

        public void Dispose()
        {
            //Close the SQL Connection and dispose the objects
            _dbTransaction.Connection?.Close();
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();
        }
    }
}
