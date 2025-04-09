namespace WebShoppingAPI.Infrastructure.Models
{
    public class Variant: BaseModel
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string? Title { get; set; }
        public string? UrlSlug { get; set; }
        public int MeasureId { get; set; }
        public decimal UnitPrice { get; set; }
        public int VariantStatusId { get; set; }
        public string? MainImagePath { get; set; }
        public List<string> AdditionalImagePaths { get; set; } = [];
        public int? TotalStocks { get; set; }
        public int? RemainingStocks { get; set; }
        public string? VariantShortName { get; set; }
        public string? VariantSKU { get; set; }

    }
}
