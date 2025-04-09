namespace WebShoppingAPI.Infrastructure.Models
{
    public class Measure: BaseModel
    {
        public required int Weight { get; set; }
        public string? Unit { get; set; }
    }
}
