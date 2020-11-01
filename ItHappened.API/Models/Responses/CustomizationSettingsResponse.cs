namespace ItHappened.Api.Models.Responses
{
    public class CustomizationSettingsResponse
    {
        public string ScaleMeasurementUnit { get; set; }
        public bool? IsPhotoRequired { get; set; }
        public bool? IsScaleRequired { get; set; }
        public bool? IsRatingRequired { get; set; }
        public bool? IsGeotagRequired { get; set; }
        public bool? IsCommentRequired { get; set; }
        public bool? IsCustomizationRequired { get; set; }
    }
}