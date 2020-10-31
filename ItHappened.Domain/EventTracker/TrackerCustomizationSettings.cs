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
        public bool IsGeoTagRequired { get; }
        public bool IsCommentRequired { get; }
        public bool IsCustomizationRequired { get; }

        public TrackerCustomizationSettings(bool isPhotoRequired,
            bool isScaleRequired,
            Option<string> scaleMeasurementUnit,
            bool isRatingRequired,
            bool isGeoTagRequired,
            bool isCommentRequired,
            bool isCustomizationRequired)
        {
            IsPhotoRequired = isPhotoRequired;
            IsScaleRequired = isScaleRequired;
            ScaleMeasurementUnit = scaleMeasurementUnit;
            IsRatingRequired = isRatingRequired;
            IsGeoTagRequired = isGeoTagRequired;
            IsCommentRequired = isCommentRequired;
            IsCustomizationRequired = isCustomizationRequired;
        }

        public TrackerCustomizationSettings()
        {
            IsPhotoRequired = false;
            IsScaleRequired = false;
            ScaleMeasurementUnit = Option<string>.None;
            IsRatingRequired = false;
            IsGeoTagRequired = false;
            IsCommentRequired = false;
            IsCustomizationRequired = false;
        }
        
        protected bool Equals(TrackerCustomizationSettings other)
        {
            return IsPhotoRequired == other.IsPhotoRequired && IsScaleRequired == other.IsScaleRequired &&
                   ScaleMeasurementUnit.Equals(other.ScaleMeasurementUnit) &&
                   IsRatingRequired == other.IsRatingRequired && IsGeoTagRequired == other.IsGeoTagRequired &&
                   IsCommentRequired == other.IsCommentRequired && IsCustomizationRequired == other.IsCustomizationRequired;
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
                IsGeoTagRequired, IsCommentRequired, IsCustomizationRequired);
        }
        
        
    }
}