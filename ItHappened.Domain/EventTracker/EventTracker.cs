using System;

namespace ItHappened.Domain
{
    public class EventTracker
    {
        public EventTracker(Guid id, Guid creatorId, string name, EventTrackerCustomizations customizations)
        {
            Id = id;
            CreatorId = creatorId;
            Name = name;
            Customizations = customizations;
        }
        
        public Guid Id { get; }
        public Guid CreatorId { get; }
        public string Name { get; }
        public EventTrackerCustomizations Customizations { get; }
    }
}