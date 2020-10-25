using LanguageExt;

namespace ItHappened.Domain
{
    public class TrackerCustomizationSettings
    {
        public Option<string> ScaleMeasurementUnit { get; }
        public bool IsPhotoRequired { get; }
        public bool IsRatingRequired { get; }
        public bool IsGeoTagRequired { get; }
        public bool IsCommentRequired { get; }
        public bool IsScaleRequired { get; }

        public bool AreCustomizationsOptional { get; }

        public TrackerCustomizationSettings(Option<string> scaleMeasurementUnit,
            bool isPhotoRequired,
            bool isRatingRequired,
            bool isGeoTagRequired,
            bool isCommentRequired,
            bool isScaleRequired,
            bool areCustomizationsOptional)
        {
            ScaleMeasurementUnit = scaleMeasurementUnit;
            IsPhotoRequired = isPhotoRequired;
            IsRatingRequired = isRatingRequired;
            IsGeoTagRequired = isGeoTagRequired;
            IsCommentRequired = isCommentRequired;
            AreCustomizationsOptional = areCustomizationsOptional;
            IsScaleRequired = isScaleRequired;
        }

        public TrackerCustomizationSettings()
        {
            AreCustomizationsOptional = false;
            IsScaleRequired = false;
            ScaleMeasurementUnit = Option<string>.None;
            IsPhotoRequired = false;
            IsRatingRequired = false;
            IsGeoTagRequired = false;
            IsCommentRequired = false;
            
        }
    }
}