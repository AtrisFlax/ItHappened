using System;

namespace ItHappend
{
    public class Event
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public EventEvaluation Evaluation { get; set; }

        public Event()
        {
            
        }
    }
}