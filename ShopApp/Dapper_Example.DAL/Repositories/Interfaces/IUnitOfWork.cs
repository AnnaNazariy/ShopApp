using Dapper_Example.DAL.Entities;
namespace Dapper_Example.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }
        IUserRepository UserRepository { get; }
        IReviewRepository ReviewRepository { get; }

        void Commit();
        void Dispose();
    }
}
