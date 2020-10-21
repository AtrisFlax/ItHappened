using System.ComponentModel.DataAnnotations;

namespace ItHappened.Api.Contracts.Responses.User
{
    public class UserRegistrationResponse
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}