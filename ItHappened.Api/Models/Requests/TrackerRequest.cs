namespace ItHappened.Api.Models.Requests
{
    public class TrackerRequest
    {
        public string Name { get; set; }
        public CustomizationSettingsRequest CustomizationSettings { get; set; }
    }
}