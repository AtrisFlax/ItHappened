using System;

namespace ItHappened.Domain
{
    public class EventTracker
    {
        public Guid Id { get; }
        public Guid CreatorId { get; }
        public string Name { get; }
        public TrackerCustomizationSettings CustomizationSettings { get; }

        public EventTracker(Guid id, Guid creatorId, string name, TrackerCustomizationSettings customizationSettings)
        {
            Id = id;
            CreatorId = creatorId;
            Name = name;
            CustomizationSettings = customizationSettings;
        }

        public bool TrackerCustomizationsAndEventCustomizationsMatch(Event @event)
        {
            return CustomizationMatch(CustomizationSettings.IsPhotoRequired, @event.CustomizationsParameters.Photo.IsSome) &&
                   CustomizationMatch(CustomizationSettings.IsCommentRequired, @event.CustomizationsParameters.Comment.IsSome) &&
                   CustomizationMatch(CustomizationSettings.IsRatingRequired, @event.CustomizationsParameters.Rating.IsSome) &&
                   CustomizationMatch(CustomizationSettings.IsGeoTagRequired, @event.CustomizationsParameters.GeoTag.IsSome) &&
                   CustomizationMatch(CustomizationSettings.ScaleMeasurementUnit.IsSome, @event.CustomizationsParameters.Scale.IsSome);
        }


        private bool CustomizationMatch(bool isTrackerRequired, bool isEventHas)
        {
            if (CustomizationSettings.ForceCustomizations)
            {
                return isTrackerRequired == isEventHas;
            }
            return true;
        }
    }
}