namespace ItHappened.Api.Contracts.Requests
{
    public class CustomizationsRequest
    {
        public bool HasPhoto { get; set; }
        public bool HasScale { get; set; }
        public bool HasRating { get; set; }
        public bool HasGeoTag { get; set; }
        public bool HasComment { get; set; }
    }
}