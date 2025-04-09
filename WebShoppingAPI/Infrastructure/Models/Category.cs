namespace WebShoppingAPI.Infrastructure.Models
{
    public class Category : BaseModel
    {
        public required string CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public int ParentId { get; set; }
        public string? UrlSlug { get; set; }
        public int CategoryStatusId { get; set; }
    }
}
