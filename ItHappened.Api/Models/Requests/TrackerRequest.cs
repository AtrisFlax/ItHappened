using FluentValidation;

namespace ItHappened.Api.Models.Requests
{
    public class TrackerRequest
    {
        public string Name { get; set; }
        public CustomizationSettingsRequest CustomizationSettings { get; set; }
    }
    
    public class TrackerRequestValidator : AbstractValidator<TrackerRequest>
    {
        public TrackerRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CustomizationSettings).NotEmpty();
        }
    }
}