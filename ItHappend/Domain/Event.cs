using System;

namespace ItHappend.Domain
{
    public class Event
    {
        public Guid EventId { get; }

        public Guid CreatorUserId { get; }

        public DateTimeOffset EventHappensDate { get; private set; }
        public string Name { get; private set; }
        public decimal Evaluation { get; private set; }

        public Event(Guid eventId, Guid creatorId, string name, DateTimeOffset eventHappensDate, decimal evaluation)
        {
            CreatorUserId = creatorId;
            EventId = eventId;
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