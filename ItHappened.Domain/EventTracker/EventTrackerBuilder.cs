using System;
using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Domain
{
    public class EventTrackerBuilder : IEventTrackerBuilder
    {
        public Guid Id { get; private set; }
        public Guid CreatorId { get; private set; }
        public string Name { get; private set; }
        public Option<string> ScaleMeasurementUnit { get; private set; }
        public bool HasPhoto { get; private set; }
        public bool HasScale { get; private set; }
        public bool HasRating { get; private set; }
        public bool HashGeoTag { get; private set; }
        public bool HasComment { get; private set; }

        public static EventTrackerBuilder Tracker(Guid creatorId, Guid trackerId, string name)
        {
            return new EventTrackerBuilder
            {
                CreatorId = creatorId,
                Id = trackerId,
                Name = name,
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

        public EventTracker Build()
        {
            return new EventTracker(this);
        }
    }
}