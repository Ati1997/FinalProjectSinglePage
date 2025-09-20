using Microsoft.AspNetCore.Mvc;
using MvcSinglePage.ApplicationServices.Dtos.productDtos;
using MvcSinglePage.ApplicationServices.Services.Contracts;

namespace MvcSinglePage.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApplicationService _productApplicationService;

        public ProductController(IProductApplicationService productApplicationService)
        {
            _productApplicationService = productApplicationService;
        }

        #region [- Index() -]
        // صفحه اصلی
        public IActionResult Index()
        {
            return View();
        } 
        #endregion

        #region [- GetAll() -]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Guard_PersonService();
            var getAllResponse = await _productApplicationService.GetAll();
            return Json(getAllResponse);
            
        }
        #endregion

        #region [- Get() -]
        public async Task<IActionResult> Get(Guid id)
        {
            Guard_PersonService();
            var getResponse = await _productApplicationService.Get(id);
            if (!getResponse.IsSuccessful)
            {
                TempData["Error"] = getResponse.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }
            return Json(getResponse);
        }
        #endregion

        #region [- Post() -]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Post_Product_Dto dto)
        {
            Guard_PersonService();
            if (dto == null)
                return BadRequest("Product data is null");

            var response = await _productApplicationService.Post(dto);

            if (!response.IsSuccessful)
                return BadRequest(response.ErrorMessage);

            return Ok(response.Result);
        }
        #endregion

        #region [- Put() -]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Put_Product_Dto dto)
        {
            Guard_PersonService();
            if (dto == null)
                return BadRequest("Product data is null");

            var response = await _productApplicationService.Put(dto);

            if (!response.IsSuccessful)
                return BadRequest(response.ErrorMessage);

            return Ok(response.Result);
        }
        #endregion

        #region [- Delete() -]
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([FromBody] Guid id)
        {
            var response = await _productApplicationService.Delete(id);

            if (!response.IsSuccessful)
                return BadRequest(new { errorMessage = response.ErrorMessage });

            return Ok(new { message = "Product deleted successfully" });
        }


        #endregion





        #region [- PersonServiceGuard() -]
        private ObjectResult Guard_PersonService()
        {
            if (_productApplicationService is null)
            {
                return Problem($" {nameof(_productApplicationService)} is null.");
            }

            return null;
        }
        #endregion
    }
}
