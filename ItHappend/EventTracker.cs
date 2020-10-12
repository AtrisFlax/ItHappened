using System;
using System.Collections.Generic;

namespace ItHappend
{
    public class EventTracker
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Event> Events { get; set; }

        public EventTracker()
        {
            
        }
    }
}