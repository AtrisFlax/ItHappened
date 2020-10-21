using System;
using System.Collections.Generic;
using System.Net.Mail;
using LanguageExt;

namespace ItHappened.Domain
{
    //public class EventTrackerBuilder : IEventTrackerBuilder
    //{
    //    private readonly EventTracker eventTracker;

    //    public static EventTrackerBuilder Tracker(Guid creatorId, Guid trackerId, string name)
    //    {
    //        return new EventTrackerBuilder
    //        {
    //            CreatorId = creatorId,
    //            Id = trackerId,
    //            Name = name,
    //        };
    //    }
        
    //    public EventTrackerBuilder WithPhoto()
    //    {
    //        eventTracker.HasPhoto = true;
    //        return this;
    //    }

    //    public EventTrackerBuilder WithScale(string measurementUnit)
    //    {
    //        HasScale = true;
    //        ScaleMeasurementUnit = Option<string>.Some(measurementUnit);
    //        return this;
    //    }

    //    public EventTrackerBuilder WithRating()
    //    {
    //        HasRating = true;
    //        return this;
    //    }

    //    public EventTrackerBuilder WithGeoTag()
    //    {
    //        HashGeoTag = true;
    //        return this;
    //    }

    //    public EventTrackerBuilder WithComment()
    //    {
    //        HasComment = true;
    //        return this;
    //    }

    //    public EventTracker Build()
    //    {
    //        return new EventTracker(this);
    //    }
    //}
}