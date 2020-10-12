using System;
using System.Collections.Generic;
using System.Linq;

namespace ItHappend
{
    public class EventTracker
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public IList<Event> Events { get; private set; }

        public EventTracker(Guid id, string name, IList<Event> events)
        {
            Id = id;
            Name = name;
            Events = events;
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