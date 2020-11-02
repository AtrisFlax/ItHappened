using System;

namespace ItHappened.Domain
{
    public class EventTracker
    {
        public Guid Id { get; }
        public Guid CreatorId { get; }
        public string Name { get; }
        public TrackerCustomizationSettings CustomizationSettings { get; }
        public bool IsUpdated { get; set; } = true;

        public EventTracker(Guid id, Guid creatorId, string name, TrackerCustomizationSettings customizationSettings)
        {
            Id = id;
            CreatorId = creatorId;
            Name = name;
            CustomizationSettings = customizationSettings;
        }

        public bool IsTrackerCustomizationAndEventCustomizationMatch(Event @event)
        {
            return IsCustomizationMatch(CustomizationSettings.IsPhotoRequired, @event.CustomizationsParameters.Photo.IsSome) &&
                   IsCustomizationMatch(CustomizationSettings.IsCommentRequired, @event.CustomizationsParameters.Comment.IsSome) &&
                   IsCustomizationMatch(CustomizationSettings.IsRatingRequired, @event.CustomizationsParameters.Rating.IsSome) &&
                   IsCustomizationMatch(CustomizationSettings.IsGeotagRequired, @event.CustomizationsParameters.GeoTag.IsSome) &&
                   IsCustomizationMatch(CustomizationSettings.ScaleMeasurementUnit.IsSome, @event.CustomizationsParameters.Scale.IsSome);
        }


        private bool IsCustomizationMatch(bool isTrackerRequired, bool isEventHas)
        {
            if (CustomizationSettings.IsCustomizationRequired)
            {
                return isTrackerRequired == isEventHas;
            }
            return true;
        }
    }
}