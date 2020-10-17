using System;
using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Domain
{
    public interface IEventTrackerBuilder
    {
        EventTrackerBuilder WithPhoto();
        EventTrackerBuilder WithScale(string measurementUnit);
        EventTrackerBuilder WithRating();
        EventTrackerBuilder WithGeoTag();
        EventTrackerBuilder WithComment();
    }
}