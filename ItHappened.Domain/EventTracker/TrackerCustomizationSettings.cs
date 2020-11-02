using System;
using LanguageExt;

namespace ItHappened.Domain
{
    public class TrackerCustomizationSettings
    {
        public bool IsPhotoRequired { get; }
        public bool IsScaleRequired { get; }
        public Option<string> ScaleMeasurementUnit { get; }
        public bool IsRatingRequired { get; }
        public bool IsGeotagRequired { get; }
        public bool IsCommentRequired { get; }
        public bool IsCustomizationRequired { get; }

        public TrackerCustomizationSettings(bool isPhotoRequired,
            bool isScaleRequired,
            Option<string> scaleMeasurementUnit,
            bool isRatingRequired,
            bool isGeotagRequired,
            bool isCommentRequired,
            bool isCustomizationRequired)
        {
            IsPhotoRequired = isPhotoRequired;
            IsScaleRequired = isScaleRequired;
            ScaleMeasurementUnit = isScaleRequired ? scaleMeasurementUnit : Option<string>.None;
            IsRatingRequired = isRatingRequired;
            IsGeotagRequired = isGeotagRequired;
            IsCommentRequired = isCommentRequired;
            IsCustomizationRequired = isCustomizationRequired;
        }

        public TrackerCustomizationSettings()
        {
            IsPhotoRequired = false;
            IsScaleRequired = false;
            ScaleMeasurementUnit = Option<string>.None;
            IsRatingRequired = false;
            IsGeotagRequired = false;
            IsCommentRequired = false;
            IsCustomizationRequired = false;
        }

        protected bool Equals(TrackerCustomizationSettings other)
        {
            return IsPhotoRequired == other.IsPhotoRequired && IsScaleRequired == other.IsScaleRequired &&
                   ScaleMeasurementUnit.Equals(other.ScaleMeasurementUnit) &&
                   IsRatingRequired == other.IsRatingRequired && IsGeotagRequired == other.IsGeotagRequired &&
                   IsCommentRequired == other.IsCommentRequired &&
                   IsCustomizationRequired == other.IsCustomizationRequired;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TrackerCustomizationSettings) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsPhotoRequired, IsScaleRequired, ScaleMeasurementUnit, IsRatingRequired,
                IsGeotagRequired, IsCommentRequired, IsCustomizationRequired);
        }
    }
}