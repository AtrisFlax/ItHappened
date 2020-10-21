namespace ItHappened.Domain
{
    public interface IEventTrackerBuilder
    {
        EventTracker.EventTrackerBuilder WithPhoto();
        EventTracker.EventTrackerBuilder WithScale(string measurementUnit);
        EventTracker.EventTrackerBuilder WithRating();
        EventTracker.EventTrackerBuilder WithGeoTag();
        EventTracker.EventTrackerBuilder WithComment();
    }
}