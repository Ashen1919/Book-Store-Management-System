using Microsoft.AspNetCore.Identity;

namespace RathnaBookStore.API.Repositories.Auth_Repository
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user);
    }
}
