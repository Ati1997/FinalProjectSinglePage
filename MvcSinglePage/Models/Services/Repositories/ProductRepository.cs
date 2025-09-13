using Microsoft.EntityFrameworkCore;
using MvcSinglePage.Models.DomainModels.ProductAggregates;
using MvcSinglePage.Models.Services.Contracts;
using ResponseFramework;
using System.Net;

namespace MvcSinglePage.Models.Services.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SinglePageDbContext _context;

        #region [-Constractor-]
        public ProductRepository(SinglePageDbContext context)
        {
            _context = context;
        }
        #endregion


        #region [-Insert-]
        public async Task<IResponse<Product>> Insert(Product product)
        {
            if (product == null)
                return new Response<Product>("Product cannot be null");

            try
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return new Response<Product>(product, true, "Product inserted successfully", null, HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return new Response<Product>(ex.Message);
            }
        }
        #endregion

        #region [-Update-]
        public async Task<IResponse<Product>> Update(Product product)
        {
            if (product == null)
                return new Response<Product>("Product cannot be null");

            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                return new Response<Product>(product, true, "Product updated successfully", null, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new Response<Product>(ex.Message);
            }
        }
        #endregion

        #region [-Delete-]
        public async Task<IResponse<Product>> Delete(Product product)
        {
            if (product == null)
                return new Response<Product>("Product cannot be null");

            try
            {
                _context.Attach(product);
                _context.Remove(product);
                await _context.SaveChangesAsync();
                return new Response<Product>(product, true, "Product deleted successfully", null, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new Response<Product>(ex.Message);
            }
        }
        #endregion

        #region [-selectAll-]

        public async Task<IResponse<List<Product>>> SelectAll()
        {
            try
            {
                var products = await _context.Product.ToListAsync();
                return new Response<List<Product>>(products);
            }
            catch (Exception ex)
            {
                return new Response<List<Product>>(ex.Message);
            }
        }
        #endregion

        #region [-SelectById-]
        public async Task<IResponse<Product>> SelectById(Guid id)
        {
            if (id == Guid.Empty)
                return new Response<Product>("Invalid product ID");

            try
            {
                var product = await _context.Product.FindAsync(id);
                return new Response<Product>(product);
            }
            catch (Exception ex)
            {
                return new Response<Product>(ex.Message);
            }
        }
        #endregion

        //private method

    }
}
