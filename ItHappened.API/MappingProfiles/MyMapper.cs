using System.Collections.Generic;
using ItHappened.Api.Models.Requests;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using LanguageExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ItHappened.Api.MappingProfiles
{
    public class MyMapper : IMyMapper
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            TypeNameHandling = TypeNameHandling.None
        };

        public EventCustomParameters GetEventCustomParametersFromRequest(EventRequest request)
        {
            var scale = request.Scale == null ? Option<double>.None : Option<double>.Some(request.Scale.Value);
            var rating = request.Rating == null ? Option<double>.None : Option<double>.Some(request.Rating.Value);
            var geoTag = request.GeoTag == null
                ? Option<GeoTag>.None
                : Option<GeoTag>.Some(new GeoTag(request.GeoTag.GpsLat, request.GeoTag.GpsLng));
            var comment = request.Comment == null
                ? Option<Comment>.None
                : Option<Comment>.Some(new Comment(request.Comment));

            var customParameters = new EventCustomParameters(
                Option<Photo>.None, //TODO issue #148
                scale,
                rating,
                geoTag,
                comment
            );
            return customParameters;
        }

        public string EventToJson(Event request)
        {
            return JsonConvert.SerializeObject(request, Formatting.Indented, _jsonSerializerSettings);
        }

        public string EventsToJson(IReadOnlyCollection<Event> events)
        {
            return JsonConvert.SerializeObject(events, Formatting.Indented, _jsonSerializerSettings);
        }

        public string SingleFactsToJson(IReadOnlyCollection<ISingleTrackerFact> facts)
        {
            return JsonConvert.SerializeObject(facts, Formatting.Indented, _jsonSerializerSettings);
        }

        public string MultipleFactsToJson(IReadOnlyCollection<IMultipleTrackersFact> facts)
        {
            return JsonConvert.SerializeObject(facts, Formatting.Indented, _jsonSerializerSettings);
        }
    }
}