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
    }
}