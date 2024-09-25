namespace Dapper_Example.DAL.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> ProductByCategoryAsync(int CategoryId);
    }
}
