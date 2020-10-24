using FluentValidation;
using ItHappened.Domain;

namespace ItHappened.Api.Models.Requests
{
    public class EventCustomParametersRequest
    {
        //public PhotoRequest Photo { get; set; }
        public double Scale { get; set; }
        public double Rating { get; set; }
        public GeoTagRequest GeoTag { get; set; }
        public string Comment { get; set; }
    }

    public class PhotoRequest
    {
        public byte[] PhotoBytes { get; set; }
    }
    
    public class GeoTagRequest
    {
        public double GpsLat { get; set; }
        public double GpsLng { get; set; }
    }
    
    public class EventCustomParametersRequestValidator : AbstractValidator<EventCustomParametersRequest>
    {
        public EventCustomParametersRequestValidator()
        {
            //RuleFor(x => x.)
        }
    }
}