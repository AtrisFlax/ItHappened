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

        public bool IsSettingsAndEventCustomizationsMatch(Event @event)
        {
            return /*CustomizationMatch(CustomizationSettings.IsPhotoRequired, @event.CustomizationsParameters.Photo.IsSome) &&*/ //TODO issue #148
                   CustomizationMatch(CustomizationSettings.IsCommentRequired, @event.CustomizationsParameters.Comment.IsSome) &&
                   CustomizationMatch(CustomizationSettings.IsRatingRequired, @event.CustomizationsParameters.Rating.IsSome) &&
                   CustomizationMatch(CustomizationSettings.IsGeoTagRequired, @event.CustomizationsParameters.GeoTag.IsSome) &&
                   CustomizationMatch(CustomizationSettings.ScaleMeasurementUnit.IsSome, @event.CustomizationsParameters.Scale.IsSome);
        }


        private bool CustomizationMatch(bool isRequired, bool isEventSome)
        {
            return isRequired && isEventSome || CustomizationSettings.AreCustomizationsOptional;
        }
    }
}