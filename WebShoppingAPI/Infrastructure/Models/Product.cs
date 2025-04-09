namespace WebShoppingAPI.Infrastructure.Models
{
    public class Product : BaseModel
    {
        public required string ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductUrlSlug { get; set; }
        public string? ProductShortName { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int ParentCategoryId { get; set; }
        public int ProductStatusId { get; set; }

    }
}
