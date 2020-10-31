using ItHappened.Domain.Statistics;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;


namespace ItHappened.Api.Mapping
{
    namespace ItHappened.Api.MappingProfiles
    {
        public class FactsToNewtonJsonMapper : IFactsToJsonMapper
        {
            public string SingleFactsToJson(IReadOnlyCollection<ISingleTrackerFact> facts)
            {
                return JsonConverterEx.SerializeObject(new
                {
                    SpecificFacts = facts
                });
            }

            public string MultipleFactsToJson(IReadOnlyCollection<IMultipleTrackersFact> facts)
            {
                return JsonConverterEx.SerializeObject(new
                {
                    GeneralFacts = facts
                });
            }

            
            private static class JsonConverterEx
            {
                public static string SerializeObject<T>(T value)
                {
                    var sb = new StringBuilder(256);
                    var sw = new StringWriter(sb, CultureInfo.InvariantCulture);
                    var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented,
                    });
                    using var jsonWrite = new JsonTextWriter(sw)
                    {
                        Indentation = 4
                    };
                    jsonSerializer.Serialize(jsonWrite, value, typeof(T));

                    return sw.ToString();
                }
            }
        }
    }
}