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
            if (@event.CustomizationsParameters.Comment.IsNone && !CustomizationSettings.CommentIsOptional)
                return false;
            if (@event.CustomizationsParameters.Photo.IsNone && !CustomizationSettings.PhotoIsOptional)
                return false;
            if (@event.CustomizationsParameters.Rating.IsNone && !CustomizationSettings.RatingIsOptional)
                return false;
            if (@event.CustomizationsParameters.GeoTag.IsNone && !CustomizationSettings.GeoTagIsOptional)
                return false;
            return true;
        }
    }
}