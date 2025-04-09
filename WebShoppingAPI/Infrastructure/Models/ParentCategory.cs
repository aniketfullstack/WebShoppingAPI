namespace WebShoppingAPI.Infrastructure.Models
{
    public class ParentCategory : BaseModel
    {
        public required string ParentCategoryName { get; set; }
        public string? ParentCategoryDescription { get; set; }
        public string? UrlSlug { get; set; }
        public int ParentCategoryStatusId { get; set; }
    }
}
