using ItHappened.Domain;

namespace ItHappened.Api.Authentication
{
    public interface IJwtIssuer
    {
        string GenerateToken(User user);
    }
}