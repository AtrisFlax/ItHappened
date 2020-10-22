using System;

namespace ItHappened.Domain
{
    public class Event
    {
        public Event(Guid id,
            Guid creatorId,
            Guid trackerId,
            DateTimeOffset happensDate,
            EventCustomParameters customizationsParameters)
        {
            Id = id;
            CreatorId = creatorId;
            TrackerId = trackerId;
            HappensDate = happensDate;
            CustomizationsParameters = customizationsParameters;
        }
        public Guid Id { get; }
        public Guid CreatorId { get; }
        public Guid TrackerId { get; }
        public DateTimeOffset HappensDate { get; }
        public EventCustomParameters CustomizationsParameters { get; }
    }
}