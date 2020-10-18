namespace ItHappened.Api.Requests
{
    public class CreateUserRequest
    {
        public CreateUserRequest(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}