using System;
using FluentValidation;
using LanguageExt;

namespace ItHappened.Api.Models.Requests
{
    public class EventRequest
    {
        public DateTimeOffset HappensDate { get; set; }
        public EventCustomParametersRequest CustomParameters { get; set; }
    }
    
    public class EventRequestValidator : AbstractValidator<EventRequest>
    {
        public EventRequestValidator()
        {
            RuleFor(x => x.HappensDate).NotEmpty();
            RuleFor(x => x.CustomParameters).NotEmpty();
        }
    }
}