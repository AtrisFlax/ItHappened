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
        public bool AreCustomizationsOptional { get; }

        public TrackerCustomizationSettings(Option<string> scaleMeasurementUnit,
            bool isPhotoRequired,
            bool isRatingRequired,
            bool isGeoTagRequired,
            bool isCommentRequired,
            bool areCustomizationsOptional)
        {
            ScaleMeasurementUnit = scaleMeasurementUnit;
            IsPhotoRequired = isPhotoRequired;
            IsRatingRequired = isRatingRequired;
            IsGeoTagRequired = isGeoTagRequired;
            IsCommentRequired = isCommentRequired;
            AreCustomizationsOptional = areCustomizationsOptional;
        }

        public TrackerCustomizationSettings(bool areCustomizationsOptional)
        {
            AreCustomizationsOptional = areCustomizationsOptional;
            ScaleMeasurementUnit = Option<string>.None;
            IsPhotoRequired = false;
            IsRatingRequired = false;
            IsGeoTagRequired = false;
            IsCommentRequired = false;
        }
    }
}