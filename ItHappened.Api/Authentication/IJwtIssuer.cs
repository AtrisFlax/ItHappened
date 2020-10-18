using ItHappened.Domain.User;

namespace ItHappened.Api.Authentication
{
    public interface IJwtIssuer
    {
        string GenerateToken(UserAuthInfo userAuthInfo);
    }
}