using FluentValidation;

namespace ItHappened.Api.Models.Requests
{
    public class EventRequestValidator : AbstractValidator<EventRequest>
    {
        public EventRequestValidator()
        {
            RuleFor(x => x.HappensDate).NotEmpty();
        }
    }
}