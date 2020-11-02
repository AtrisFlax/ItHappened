using System;
using ItHappened.Domain;
using ItHappened.Infrastructure.Mappers;
using LanguageExt;

namespace ItHappened.Infrastructure.Dto
{
    public class WorstRatingEventFactDto : FactDto
    {
        public double WorstRating { get; set; }
        public DateTimeOffset WorstEventDate { get; set; }
        public Comment WorstEventComment { get; set; }
        public Guid WorstEventId { get; set; } 
    }
}