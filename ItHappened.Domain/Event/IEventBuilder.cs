using System;

namespace ItHappened.Domain
{
    public interface IEventBuilder
    {
        Guid Id { get; }
        Guid CreatorId { get; }
        DateTimeOffset HappensDate { get; }
        string Title { get; }
        Event Build();
        Event.EventBuilder WithPhoto(Photo photo);
        Event.EventBuilder WithScale(double scale);
        Event.EventBuilder WithRating(double rating);
        Event.EventBuilder WithGeoTag(GeoTag geoTag);
        Event.EventBuilder WithComment(string comment);
    }
}