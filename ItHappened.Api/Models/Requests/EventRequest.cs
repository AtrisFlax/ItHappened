using System;
using FluentValidation;

namespace ItHappened.Api.Models.Requests
{
    public class EventRequest
    {
        public DateTimeOffset HappensDate { get; set; }
        //public PhotoRequest Photo { get; set; }
        public double Scale { get; set; }
        public double Rating { get; set; }
        public GeoTagRequest GeoTag { get; set; }
        public string Comment { get; set; }
    }

    public class EventsRequest
    {
        public EventRequest[] Events { get; set; }
    }

    public class GeoTagRequest
    {
        public double GpsLat { get; set; }
        public double GpsLng { get; set; }
    }

    public class EventRequestValidator : AbstractValidator<EventRequest>
    {
        public EventRequestValidator()
        {
            RuleFor(x => x.HappensDate).NotEmpty();
        }
    }
}