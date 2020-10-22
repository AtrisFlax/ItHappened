using System;

namespace ItHappened.Api.Contracts.Responses
{
    public class TrackerResponse
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public string Name { get; set; }
        public CustomizationSettings CustomizationSettings { get; set; }
    }

    public class CustomizationSettings
    {
        public bool PhotoIsOptional { get; set; }
        public bool RatingIsOptional { get; set; }
        public bool GeoTagIsOptional { get; set; }
        public bool CommentIsOptional { get; set; }
        public string ScaleMeasurementUnit;
    }
}