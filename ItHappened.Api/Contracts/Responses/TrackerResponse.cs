using System;
using ItHappened.Api.Contracts.Requests;
using LanguageExt;

namespace ItHappened.Api.Contracts.Responses
{
    public class TrackerResponse
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public string Name { get; set; }
        public CustomizationsResponse Customizations { get; set; }
    }

    public class CustomizationsResponse
    {
        public bool HasPhoto { get; set; }
        public bool HasRating { get; set; }
        public bool HasGeoTag { get; set; }
        public bool HasComment { get; set; }
        public string ScaleMeasurementUnit;
    }
}