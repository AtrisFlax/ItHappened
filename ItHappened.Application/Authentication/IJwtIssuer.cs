using ItHappened.Domain;

namespace ItHappened.Application.Authentication
{
    public interface IJwtIssuer
    {
        string GenerateToken(User user);
    }
}