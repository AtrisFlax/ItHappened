namespace ItHappened.Api.Contracts
{
    public class LoginResponse
    {
        public LoginResponse(string accessToken)
        {
            AccessToken = accessToken;
        }

        public string AccessToken { get; }
    }
}