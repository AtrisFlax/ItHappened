using System;
using System.Collections.Generic;

namespace ItHappend.Domain
{
    public class EventTracker
    {
        public Guid Id { get; }
        public string Name { get; }
        public IList<Event> Events { get; private set; }

        public Guid CreatorId { get; } 
        
        public EventTracker(Guid id, string name, IList<Event> events, Guid creatorId)
        {
            Id = id;
            Name = name;
            Events = events;
            CreatorId = creatorId;
        }

        public void AddEvent(Event newEvent)
        {
            Events.Add(newEvent);
        }
        
        public bool RemoveEvent(Event newEvent)
        {
            return Events.Remove(newEvent);
        }
        
    }
}