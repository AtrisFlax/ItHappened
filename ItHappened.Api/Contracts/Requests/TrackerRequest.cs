using LanguageExt;

namespace ItHappened.Api.Contracts.Requests
{
    public class TrackerRequest
    {
        public string Name { get; set; }
        public CustomizationSettingsRequest CustomizationSettings { get; set; }
        public Option<string> ScaleMeasurementUnit;
    }
}