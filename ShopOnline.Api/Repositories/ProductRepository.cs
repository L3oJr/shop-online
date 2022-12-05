using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Interfaces;

namespace ShopOnline.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public ProductRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            return await this.shopOnlineDbContext.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            return await shopOnlineDbContext.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Product> GetItem(int id)
        {
            return await shopOnlineDbContext.Products
                .Include(product => product.ProductCategory)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            return await this.shopOnlineDbContext.Products
                .Include(product => product.ProductCategory)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
        {
            var products = await shopOnlineDbContext.Products
                .Include(product => product.ProductCategory)
                .Where(product => product.CategoryId == id)
                .ToArrayAsync();

            return products;
        }
    }
}
