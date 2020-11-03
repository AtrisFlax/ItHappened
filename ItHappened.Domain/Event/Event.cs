using System;

namespace ItHappened.Domain
{
    public class Event
    {
        public Guid Id { get; }
        public Guid CreatorId { get; }
        public Guid TrackerId { get; }
        public DateTimeOffset HappensDate { get; }
        public string Title { get; }
        public EventCustomParameters CustomizationsParameters { get; }

        public Event(Guid id,
            Guid creatorId,
            Guid trackerId,
            DateTimeOffset happensDate,
            string title,
            EventCustomParameters customizationsParameters)
        {
            Id = id;
            CreatorId = creatorId;
            TrackerId = trackerId;
            HappensDate = happensDate;
            Title = title;
            CustomizationsParameters = customizationsParameters;
        }
    }
}