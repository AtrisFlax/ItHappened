using Newtonsoft.Json;

namespace ItHappend.StatisticService
{
    public interface IJsonService
    {
        string SerializeObject(object value);
    }

    class JsonService : IJsonService
    {
        public string SerializeObject(object value)
        {
            return  JsonConvert.SerializeObject(value, Formatting.Indented);
        }
    }
}