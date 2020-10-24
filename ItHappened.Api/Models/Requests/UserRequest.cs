using FluentValidation;

namespace ItHappened.Api.Models.Requests
{
    public class UserRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
    
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Password).Password();
        }
    }
}