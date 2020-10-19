using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Serilog;

namespace ItHappened.Domain
{
    public class EventTracker
    {
        public Guid Id { get; }
        public string Name { get; }
        public Guid CreatorId { get; }

        public bool HasPhoto { get; }
        public bool HasScale { get; }
        public bool HasRating { get; }
        public bool HashGeoTag { get; }
        public bool HasComment { get; }

        public Option<string> ScaleMeasurementUnit;

        public EventTracker(Guid creatorId,
            Guid id,
            string name,
            IList<Event> events,
            bool hasPhoto = false,
            bool hasScale = false,
            bool hasRating = false,
            bool hashGeoTag = false,
            bool hasComment = false)
        {
            Id = id;
            Name = name;
            CreatorId = creatorId;
            HasPhoto = hasPhoto;
            HasScale = hasScale;
            HasRating = hasRating;
            HashGeoTag = hashGeoTag;
            HasComment = hasComment;
        }

        public EventTracker(EventTrackerBuilder eventTrackerBuilder)
        {
            Id = eventTrackerBuilder.Id;
            Name = eventTrackerBuilder.Name;
            CreatorId = eventTrackerBuilder.CreatorId;
            HasPhoto = eventTrackerBuilder.HasPhoto;
            HasScale = eventTrackerBuilder.HasScale;
            ScaleMeasurementUnit = eventTrackerBuilder.ScaleMeasurementUnit;
            HasRating = eventTrackerBuilder.HasRating;
            HashGeoTag = eventTrackerBuilder.HashGeoTag;
            HasComment = eventTrackerBuilder.HasComment;
        }
        
        //TODO убрать как сделаю замену этим функциям
        // public bool AddEvent(Event newEvent)
        // {
        //     if (IsTrackerAndEventCustomizationsConfirm(newEvent))
        //     {
        //         Log.Information("Cant add event, wrong customization");
        //         return false;
        //     }
        //
        //     return true;
        // }
        //
        // public void RemoveEvent(Event eventToRemove)
        // {
        //     Events.Remove(eventToRemove);
        // }
        //
        // public IReadOnlyCollection<Event> FilterEventsByTimeSpan(DateTimeOffset from, DateTimeOffset to)
        // {
        //     var filteredEvents = Events.Where(eventItem =>
        //         eventItem.HappensDate.UtcDateTime >= from.UtcDateTime &&
        //         eventItem.HappensDate.UtcDateTime <= to.UtcDateTime).ToArray();
        //     return filteredEvents;
        // }

        private bool IsTrackerAndEventCustomizationsConfirm(Event newEvent)
        {
            if (HasPhoto != newEvent.Photo.IsSome) return true;
            if (HasScale != newEvent.Scale.IsSome) return true;
            if (HasRating != newEvent.Rating.IsSome) return true;
            if (HashGeoTag != newEvent.GeoTag.IsSome) return true;
            return HasComment != newEvent.Comment.IsSome;
        }
    }
}