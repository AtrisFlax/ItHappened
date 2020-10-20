using System.ComponentModel.DataAnnotations;

namespace ItHappened.Api.Contracts.Requests.User
{
    public class UserRegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}