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

        public bool SettingsAndEventCustomizationsMatch(Event @event)
        {
            return CustomizationSettings.IsPhotoRequired &&
             (@event.CustomizationsParameters.Photo.IsSome || CustomizationSettings.AreCustomizationsOptional) &&
                   CustomizationSettings.IsCommentRequired &&
                    (@event.CustomizationsParameters.Comment.IsSome || CustomizationSettings.AreCustomizationsOptional) &&
                   CustomizationSettings.IsRatingRequired &&
                    (@event.CustomizationsParameters.Rating.IsSome || CustomizationSettings.AreCustomizationsOptional) &&
                   CustomizationSettings.IsGeoTagRequired &&
                   (@event.CustomizationsParameters.GeoTag.IsSome || CustomizationSettings.AreCustomizationsOptional) &&
                   CustomizationSettings.ScaleMeasurementUnit.IsSome &&
                   (@event.CustomizationsParameters.Scale.IsSome || CustomizationSettings.AreCustomizationsOptional);
        }
    }
}