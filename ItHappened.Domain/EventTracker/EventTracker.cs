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
        public bool HasGeoTag { get; }
        public bool HasComment { get; }

        public Option<string> ScaleMeasurementUnit;

        public EventTracker(Guid creatorId,
            Guid id,
            string name,
            bool hasPhoto,
            bool hasScale,
            bool hasRating,
            bool hasGeoTag,
            bool hasComment)
        {
            Id = id;
            Name = name;
            CreatorId = creatorId;
            HasPhoto = hasPhoto;
            HasScale = hasScale;
            HasRating = hasRating;
            HasGeoTag = hasGeoTag;
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
            HasGeoTag = eventTrackerBuilder.HashGeoTag;
            HasComment = eventTrackerBuilder.HasComment;
        }
        
        public bool IsTrackerAndEventCustomizationsMatch(Event newEvent)
        {
            if (HasPhoto != newEvent.Photo.IsSome) return false;
            if (HasScale != newEvent.Scale.IsSome) return false;
            if (HasRating != newEvent.Rating.IsSome) return false;
            if (HasGeoTag != newEvent.GeoTag.IsSome) return false;
            return HasComment == newEvent.Comment.IsSome;
        }
    }
}