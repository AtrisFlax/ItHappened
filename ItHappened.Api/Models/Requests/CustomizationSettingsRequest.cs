namespace ItHappened.Api.Models.Requests
{
    public class CustomizationSettingsRequest
    {
        public string ScaleMeasurementUnit { get; set; }
        public bool IsPhotoRequired { get; set; }
        public bool IsScaleRequired { get; set; }
        public bool IsRatingRequired { get; set; }
        public bool IsGeotagRequired { get; set; }
        public bool IsCommentRequired { get; set; }
        public bool AreCustomizationsOptional { get; set; }
    }
    
}