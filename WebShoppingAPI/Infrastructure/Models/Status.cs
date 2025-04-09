using System.ComponentModel.DataAnnotations.Schema;

namespace WebShoppingAPI.Infrastructure.Models
{
    public class Status : BaseModel
    {
        public required string StatusName { get; set; }
    }
}
