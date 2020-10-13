namespace ItHappend.Domain
{
    public interface IEventBuilder
    {
        EventBuilder WithPhoto(Photo photo);
        EventBuilder WithScale(double scale);
        EventBuilder WithRating(double rating);
        EventBuilder WithGeoTag(double latitude, double longitude);
        EventBuilder WithComment(string text);
    }
}