using System;

namespace ItHappend.Domain
{
    public class Event
    {
        public Guid Id { get; }
        public Guid CreatorId { get; }
        public DateTimeOffset EventHappensDate { get; private set; }
        public string Name { get; private set; }
        public decimal Evaluation { get; private set; }

        public Event(Guid id, Guid creatorId, string name, DateTimeOffset eventHappensDate, decimal evaluation)
        {
            CreatorId = creatorId;
            Id = id;
            Name = name;
            EventHappensDate = eventHappensDate;
            Evaluation = evaluation;
        }

        public void EditEvent(string name, DateTimeOffset eventHappensDate, decimal evaluation)
        {
            Name = name;
            EventHappensDate = eventHappensDate;
            Evaluation = evaluation;
        }
    }
}