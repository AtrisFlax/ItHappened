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

        public bool HasPhoto { get; private set; }
        public bool HasScale { get; private set; }
        public bool HasRating { get;  private set; }
        public bool HasGeoTag { get; private set; }
        public bool HasComment { get; private set; }
        public Guid EventTrackerId { get; private set; }

        public Option<string> ScaleMeasurementUnit;

        private EventTracker(Guid eventTrackerId, Guid creatorId, string name)
        {
            EventTrackerId = eventTrackerId;
            CreatorId = creatorId;
            Name = name;
        }

        public static EventTrackerBuilder Create(Guid eventTrackerId, Guid creatorId, string Name)
        {
            return new EventTrackerBuilder(new EventTracker(eventTrackerId, creatorId, Name));
        }

        public class EventTrackerBuilder : IEventTrackerBuilder
        {
            private readonly EventTracker _eventTracker;

            internal EventTrackerBuilder(EventTracker eventTracker)
            {
                _eventTracker = eventTracker;
            }
            public EventTrackerBuilder WithPhoto()
            {
                _eventTracker.HasPhoto = true;
                return this;
            }

            public EventTrackerBuilder WithScale(string measurementUnit)
            {
                _eventTracker.HasScale = true;
                _eventTracker.ScaleMeasurementUnit = Option<string>.Some(measurementUnit);
                return this;
            }

            public EventTrackerBuilder WithRating()
            {
                _eventTracker.HasRating = true;
                return this;
            }

            public EventTrackerBuilder WithGeoTag()
            {
                _eventTracker.HasGeoTag = true;
                return this;
            }

            public EventTrackerBuilder WithComment()
            {
                _eventTracker.HasComment = true;
                return this;
            }

            public EventTracker Build()
            {
                return _eventTracker;
            }
        }

        public bool IsTrackerAndEventCustomizationsMatch(Event newEvent)
            => HasPhoto == newEvent.Photo.IsSome
            && HasScale == newEvent.Scale.IsSome
            && HasRating == newEvent.Rating.IsSome
            && HasGeoTag == newEvent.GeoTag.IsSome
            && HasComment == newEvent.Comment.IsSome;
    }
}