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
        EventBuilder WithPhoto(Photo photo);
        EventBuilder WithScale(double scale);
        EventBuilder WithRating(double rating);
        EventBuilder WithGeoTag(GeoTag geoTag);
        EventBuilder WithComment(string comment);
    }
}