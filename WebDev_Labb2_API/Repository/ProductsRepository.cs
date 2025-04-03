using Microsoft.EntityFrameworkCore;
using WebDev_Labb2_API.Model;

namespace WebDev_Labb2_API.Repository
{
    public class ProductsRepository : Repository<Products>, IProductsRepository
    {
        private readonly ProdMethods _prodMethods;
        public ProductsRepository(DBContext context) : base(context)
        {
            _prodMethods = new ProdMethods();
        }

        public async Task<Products> GetBySkuAsync(int sku)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.sku == sku);
        }

        public async Task<Products> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.name == name);
        }

        public async Task<IEnumerable<Products>> GetProductsByCategory(string category)
        {
            return await _dbSet.Where(p => p.category == category).ToListAsync();
        }
        public async Task<Products> UpdateProductAsync(Products updatedProduct)
        {
            var existingProduct = await GetBySkuAsync(updatedProduct.sku);
            if (existingProduct == null)
            {
                return null;
            }

            existingProduct = _prodMethods.AssignProductValues(existingProduct, updatedProduct);

            await _context.SaveChangesAsync();
            return existingProduct;
        }
    }
}
