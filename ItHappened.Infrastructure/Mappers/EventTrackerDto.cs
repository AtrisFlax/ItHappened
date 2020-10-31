using System;

namespace ItHappened.Infrastructure.Mappers
{
    public class EventTrackerDto
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }

        public string Name { get; set; }

        public bool? IsPhotoRequired { get; set; }
        public bool? IsScaleRequired { get; set; }
        public bool? IsRatingRequired { get; set; }
        public bool? IsGeotagRequired { get; set; }
        public bool? IsCommentRequired { get; set; }
        public bool? IsCustomizationRequired { get; set; }
        
    }
}