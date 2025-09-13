using MvcSinglePage.ApplicationServices.Dtos.productDtos;
using ResponseFramework;

namespace MvcSinglePage.ApplicationServices.Services.Contracts
{
    public interface IProductApplicationService
    {

        Task<IResponse<GetAll_Product_Dto>> GetAll();
        Task<IResponse<GetById_Product_Dto>> Get(Guid id);
        Task<IResponse<GetById_Product_Dto>> Post(Post_Product_Dto product_Dto);
        Task<IResponse<GetById_Product_Dto>> Put(Put_Product_Dto product_Dto);
        Task<IResponse<bool>> Delete(Guid id);
    }
}
