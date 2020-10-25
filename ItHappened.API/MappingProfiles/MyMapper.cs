using ItHappened.Api.Models.Requests;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Api.MappingProfiles
{
    public interface IMyMapper
    {
        EventCustomParameters GetEventCustomParametersFromRequest(EventRequest request);
    }

    public class MyMapper : IMyMapper
    {
        public EventCustomParameters GetEventCustomParametersFromRequest(EventRequest request)
        {
            var customParameters = new EventCustomParameters(
                null,
                Option<double>.Some(request.Scale),
                Option<double>.Some(request.Rating),
                Option<GeoTag>.Some(new GeoTag(request.GeoTag.GpsLat,
                    request.GeoTag.GpsLng)),
                Option<Comment>.Some(new Comment(request.Comment))
            );
            return customParameters;
        }
    }
}