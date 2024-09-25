using Dapper_Example.DAL.Repositories.Interfaces;
using System.Data;
using Dapper_Example.DAL.Entities;
using Dapper_Example_Project.Connection;

namespace Dapper_Example.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IProductRepository ProductRepository { get; }
        public IUserRepository UserRepository { get; }
        public IReviewRepository ReviewRepository { get; }

        private readonly IDbTransaction _dbTransaction;

        public UnitOfWork(IProductRepository productRepository, IUserRepository userRepository,
            IReviewRepository reviewRepository, IDbTransaction dbTransaction)
        {
            ProductRepository = productRepository;
            UserRepository = userRepository;
            ReviewRepository = reviewRepository;
            _dbTransaction = dbTransaction;
        }

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
            }
            catch
            {
                _dbTransaction.Rollback();
            }
        }

        public void Dispose()
        {
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();
        }
    }
}
