namespace WebShoppingAPI.Infrastructure.Models
{
    public class Brand : BaseModel
    {
        public required string BrandName { get; set; }
        public int BrandStatusId { get; set; }
    }
}
