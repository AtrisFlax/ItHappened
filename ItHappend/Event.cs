using System;

namespace ItHappend
{
    public class Event
    {
        public Guid Id { get; }
        public DateTime CreationDate { get; }
        public string Name { get; set; }
        public EventEvaluation Evaluation { get; private set; }

        public Event(Guid id, string name, DateTime creationDate)
        {
            Id = id;
            Name = name;
            CreationDate = creationDate;
        }

        public void EvaluateEvent(EventEvaluation evaluation)
        {
            Evaluation = evaluation;
        }
    }
}