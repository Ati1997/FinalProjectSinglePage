namespace MvcSinglePage.ApplicationServices.Dtos.productDtos
{
    public class Put_Product_Dto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ProductDescription { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
