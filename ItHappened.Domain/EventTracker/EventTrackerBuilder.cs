using System;
using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Domain
{
    public class EventTrackerBuilder : IEventTrackerBuilder
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public IList<Event> Events { get; private set; }
        public Guid CreatorId { get; private set; }

        public bool HasPhoto { get; private set; }
        public bool HasScale { get; private set; }
        public Option<String> ScaleMeasurementUnit { get; private set; }
        public bool HasRating { get; private set; }
        public bool HashGeoTag { get; private set; }
        public bool HasComment { get; private set; }

        public EventTracker Build()
        {
            return new EventTracker(this);
        }
        
        public static EventTrackerBuilder TrackerEmpty(Guid id, Guid creatorId, string name)
        {
            if (name == null) throw new NullReferenceException();
            return new EventTrackerBuilder
            {
                Id = id,
                Name = name,
                Events = new List<Event>(),
                CreatorId = creatorId
            };
        }

        public static EventTrackerBuilder TrackerWithEvents(Guid id, Guid creatorId, string name, IList<Event> events)
        {
            if (name == null) throw new NullReferenceException();
            return new EventTrackerBuilder
            {
                Id = id,
                Name = name,
                Events = events,
                CreatorId = creatorId
            };
        }

        public EventTrackerBuilder WithPhoto()
        {
            HasPhoto = true;
            return this;
        }

        public EventTrackerBuilder WithScale(string measurementUnit)
        {
            HasScale = true;
            ScaleMeasurementUnit = Option<string>.Some(measurementUnit);
            return this;
        }

        public EventTrackerBuilder WithRating()
        {
            HasRating = true;
            return this;
        }

        public EventTrackerBuilder WithGeoTag()
        {
            HashGeoTag = true;
            return this;
        }

        public EventTrackerBuilder WithComment()
        {
            HasComment = true;
            return this;
        }
    }
}