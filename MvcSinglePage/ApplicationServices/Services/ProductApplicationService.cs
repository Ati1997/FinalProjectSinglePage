using MvcSinglePage.ApplicationServices.Dtos.productDtos;
using MvcSinglePage.ApplicationServices.Services.Contracts;
using MvcSinglePage.Models.DomainModels.ProductAggregates;
using MvcSinglePage.Models.Services.Contracts;
using ResponseFramework;

namespace MvcSinglePage.ApplicationServices.Services
{
    public class ProductApplicationService : IProductApplicationService
    {
        private readonly IProductRepository _productRepository;

        #region [-Constructor]
        public ProductApplicationService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        #endregion

        #region [-Post-]
        //public async Task Post(Post_Product_Dto product_Dto)
        //{
        //    if (product_Dto == null)
        //        throw new ArgumentNullException(nameof(product_Dto));

        //    if (string.IsNullOrWhiteSpace(product_Dto.Title))
        //        throw new ArgumentException("Title is required");

        //    if (product_Dto.UnitPrice <= 0)
        //        throw new ArgumentException("Price must be greater than zero");

        //    var product = new Product
        //    {
        //        Id = Guid.NewGuid(),
        //        Title = product_Dto.Title,
        //        ProductDescription = product_Dto.ProductDescription,
        //        UnitPrice = product_Dto.UnitPrice
        //    };
        //    //Task Insert(Product product);

        //    await _productRepository.Insert(product);
        //} 
        public async Task<IResponse<GetById_Product_Dto>> Post(Post_Product_Dto product_Dto)
        {
            if (product_Dto == null)
                return new Response<GetById_Product_Dto>("Product data is null");

            if (string.IsNullOrWhiteSpace(product_Dto.Title))
                return new Response<GetById_Product_Dto>("Title is required");

            if (product_Dto.UnitPrice <= 0)
                return new Response<GetById_Product_Dto>("UnitPrice must be greater than zero");

            var product = new Product
            {
                //Id = Guid.NewGuid(),
                Title = product_Dto.Title,
                ProductDescription = product_Dto.ProductDescription,
                UnitPrice = product_Dto.UnitPrice
            };

            await _productRepository.Insert(product);

            var dto = new GetById_Product_Dto
            {
                Id = product.Id,
                Title = product.Title,
                ProductDescription = product.ProductDescription,
                UnitPrice = product.UnitPrice
            };

            return new Response<GetById_Product_Dto>(dto);
        }
        #endregion

        #region [-Put-]
        //public async Task Put(Put_Product_Dto product_Dto)
        //{
        //    if (product_Dto == null || product_Dto.Id == Guid.Empty)
        //        throw new ArgumentException("Invalid product data");

        //    var product = await _productRepository.SelectById(product_Dto.Id);
        //    if (product == null)
        //        throw new InvalidOperationException("Product not found");


        //    product.Title = product_Dto.Title;
        //    product.ProductDescription = product_Dto.ProductDescription;
        //    product.UnitPrice = product_Dto.UnitPrice;

        //    await _productRepository.Update(product);
        //}
        public async Task<IResponse<GetById_Product_Dto>> Put(Put_Product_Dto product_Dto)
        {
            if (product_Dto == null || product_Dto.Id == Guid.Empty)
                return new Response<GetById_Product_Dto>("Invalid product data");

            var response = await _productRepository.SelectById(product_Dto.Id);
            if (!response.IsSuccessful || response.Result == null)
                return new Response<GetById_Product_Dto>("Product not found");

            var product = response.Result;

            product.Title = product_Dto.Title;
            product.ProductDescription = product_Dto.ProductDescription;
            product.UnitPrice = product_Dto.UnitPrice;

            var updateResponse = await _productRepository.Update(product);

            if (!updateResponse.IsSuccessful)
                return new Response<GetById_Product_Dto>("Failed to update product");

            var dto = new GetById_Product_Dto
            {
                Id = product.Id,
                Title = product.Title,
                ProductDescription = product.ProductDescription,
                UnitPrice = product.UnitPrice
            };

            return new Response<GetById_Product_Dto>(dto);
        }


        #endregion

        #region [-Delete-]
        //public async Task Delete(Guid id)
        //{
        //    if (id == Guid.Empty)
        //        throw new ArgumentException("Invalid product ID");

        //    var product = await _productRepository.SelectById(id);
        //    if (product == null)
        //        throw new InvalidOperationException("Product not found");

        //    await _productRepository.Delete(product);

        //}
        public async Task<IResponse<bool>> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return new Response<bool>("Invalid product ID");

            var response = await _productRepository.SelectById(id);

            if (!response.IsSuccessful || response.Result == null)
                return new Response<bool>("Product not found");

            var deleteResponse = await _productRepository.Delete(response.Result);

            if (!deleteResponse.IsSuccessful)
                return new Response<bool>("Failed to delete product");

            return new Response<bool>(true);
        }
        #endregion

        #region [-Get-]
        public async Task<IResponse<GetById_Product_Dto>> Get(Guid id)
        {
            if (id == Guid.Empty)
                return new Response<GetById_Product_Dto>("Invalid ID");

            var response = await _productRepository.SelectById(id);

            if (!response.IsSuccessful || response.Result == null)
                return new Response<GetById_Product_Dto>("Product not found");

            var product = response.Result;

            var dto = new GetById_Product_Dto
            {
                Id = product.Id,
                Title = product.Title,
                ProductDescription = product.ProductDescription,
                UnitPrice = product.UnitPrice
            };

            return new Response<GetById_Product_Dto>(dto);
        }

        #endregion

        #region [-GetAll-]
        //public async Task<GetAll_Product_Dto> GetAll()
        //{
        //    var products = await _productRepository.SelectAll();

        //    if (products == null || !products.Any())
        //        return new GetAll_Product_Dto { getById_Product_Dtos = new List<GetById_Product_Dto>() };

        //    return new GetAll_Product_Dto
        //    {
        //        getById_Product_Dtos = products.Select(p => new GetById_Product_Dto
        //        {
        //            Id = p.Id,
        //            Title = p.Title,
        //            ProductDescription = p.ProductDescription,
        //            UnitPrice = p.UnitPrice
        //        }).ToList()
        //    };
        //}
        public async Task<IResponse<GetAll_Product_Dto>> GetAll()
        {
            var response = await _productRepository.SelectAll();

            if (!response.IsSuccessful || response.Result == null || !response.Result.Any())
                return new Response<GetAll_Product_Dto>(new GetAll_Product_Dto { getById_Product_Dtos = new List<GetById_Product_Dto>() });

            var dto = new GetAll_Product_Dto
            {
                getById_Product_Dtos = response.Result.Select(p => new GetById_Product_Dto
                {
                    Id = p.Id,
                    Title = p.Title,
                    ProductDescription = p.ProductDescription,
                    UnitPrice = p.UnitPrice
                }).ToList()
            };

            return new Response<GetAll_Product_Dto>(dto);
        }

        #endregion
    }
}
