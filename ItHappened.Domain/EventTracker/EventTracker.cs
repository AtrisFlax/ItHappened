using System;

namespace ItHappened.Domain
{
    public class EventTracker
    {
        public EventTracker(Guid id, Guid creatorId, string name, TrackerCustomizationsSettings customizationsSettings)
        {
            Id = id;
            CreatorId = creatorId;
            Name = name;
            CustomizationsSettings = customizationsSettings;
        }
        
        public Guid Id { get; }
        public Guid CreatorId { get; }
        public string Name { get; }
        public TrackerCustomizationsSettings CustomizationsSettings { get; }

        public bool SettingsAndEventCustomizationsMatch(Event @event)
        {
            if (@event.CustomizationsParameters.Comment.IsNone && !CustomizationsSettings.CommentIsOptional)
                return false;
            if (@event.CustomizationsParameters.Photo.IsNone && !CustomizationsSettings.PhotoIsOptional)
                return false;
            if (@event.CustomizationsParameters.Rating.IsNone && !CustomizationsSettings.RatingIsOptional)
                return false;
            if (@event.CustomizationsParameters.GeoTag.IsNone && !CustomizationsSettings.GeoTagIsOptional)
                return false;
            
            return true;
        }
    }
}