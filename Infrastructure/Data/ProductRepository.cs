using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            this._context = context;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            //Eager Loading
            return await _context.Products
            //builds a query
            .Include(p => p.ProductType)
            .Include(p => p.ProductBrand)
            //Takes just one result
            .FirstOrDefaultAsync(p => id == p.Id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            //Eager Loading
            return await _context.Products
            //builds a query
            .Include(p => p.ProductType)
            .Include(p => p.ProductBrand)
            //makes it a list
            .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await this._context.ProductTypes.ToListAsync();
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await this._context.ProductBrands.ToListAsync();
        }
    }
}