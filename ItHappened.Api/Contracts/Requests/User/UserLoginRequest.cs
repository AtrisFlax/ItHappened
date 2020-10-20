namespace ItHappened.Api.Contracts.Requests.User
{
    public class UserLoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}