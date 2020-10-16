using System;
using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain
{
    public class EventTracker
    {
        public Guid Id { get; }
        public string Name { get; }
        public IList<Event> Events { get; }

        public Guid CreatorId { get; }

        public bool HasPhoto { get; }
        public bool HasScale { get; }
        public bool HasRating { get; }
        public bool HashGeoTag { get; }
        public bool HasComment { get; }

        public EventTracker(Guid id, string name, IList<Event> events, Guid creatorId,
            bool hasPhoto = false,
            bool hasScale = false,
            bool hasRating = false,
            bool hashGeoTag = false,
            bool hasComment = false)
        {
            Id = id;
            Name = name;
            Events = events;
            CreatorId = creatorId;
            HasPhoto = hasPhoto;
            HasScale = hasScale;
            HasRating = hasRating;
            HashGeoTag = hashGeoTag;
            HasComment = hasComment;
        }

        public bool TryAddEvent(Event newEvent)
        {
            if (IsTrackerAndEventCustomizationsConfirm(newEvent))
            {
                //TODO Log.Warning and return 
                return false;
            }

            Events.Add(newEvent);
            //TODO Log.Verbose and return 
            return true;
        }

        private bool IsTrackerAndEventCustomizationsConfirm(Event newEvent)
        {
            if (HasPhoto != newEvent.Photo.IsSome)
            {
                return true;
            }

            if (HasScale != newEvent.Scale.IsSome)
            {
                return true;
            }

            if (HasRating != newEvent.Rating.IsSome)
            {
                return true;
            }

            if (HashGeoTag != newEvent.GeoTag.IsSome)
            {
                return true;
            }

            return HasComment != newEvent.Comment.IsSome;
        }

        public void RemoveEvent(Event eventToRemove)
        {
            //TODO Log.Verbose and return 
            Events.Remove(eventToRemove);
        }

        public IReadOnlyCollection<Event> FilterEventsByTimeSpan(DateTimeOffset from, DateTimeOffset to)
        {
            var filteredEvents = Events.Where(eventItem =>
                eventItem.HappensDate.UtcDateTime >= from.UtcDateTime &&
                eventItem.HappensDate.UtcDateTime <= to.UtcDateTime).ToArray();
            return filteredEvents;
        }
    }
}