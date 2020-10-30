using ItHappened.Domain.Statistics;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace ItHappened.Api.Mapping
{
    namespace ItHappened.Api.MappingProfiles
    {
        public class FactsToNewtonJsonMapper : IFactsToJsonMapper
        {
            private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                TypeNameHandling = TypeNameHandling.None
            };

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
}