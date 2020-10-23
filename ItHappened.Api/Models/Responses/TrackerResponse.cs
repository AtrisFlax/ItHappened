using System;

namespace ItHappened.Api.Models.Responses
{
    public class TrackerResponse
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public string Name { get; set; }
        public CustomizationSettingsResponse CustomizationSettings { get; set; }
    }

    public class CustomizationSettingsResponse
    {
        public bool PhotoIsOptional { get; set; }
        public bool RatingIsOptional { get; set; }
        public bool GeoTagIsOptional { get; set; }
        public bool CommentIsOptional { get; set; }
        public string ScaleMeasurementUnit;
    }
}