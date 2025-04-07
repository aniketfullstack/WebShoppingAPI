using WebShoppingAPI.Infrastructure.Models.IdentityModels;

namespace WebShoppingAPI.Infrastructure.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
