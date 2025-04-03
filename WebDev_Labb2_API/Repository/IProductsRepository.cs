using WebDev_Labb2_API.Model;

namespace WebDev_Labb2_API.Repository
{
    public interface IProductsRepository : IRepository<Products>
    {
        Task<Products> GetBySkuAsync(int sku);
        Task<Products> GetByNameAsync(string name);
        Task<IEnumerable<Products>> GetProductsByCategory(string category);
        Task<Products> UpdateProductAsync(Products updatedProduct);
    }
}
