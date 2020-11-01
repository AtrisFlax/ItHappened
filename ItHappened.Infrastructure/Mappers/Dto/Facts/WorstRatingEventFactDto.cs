using System;
using ItHappened.Domain;
using ItHappened.Infrastructure.Mappers;
using LanguageExt;

namespace ItHappened.Infrastructure.Dto
{
    public class WorstRatingEventFactDto : FactDto
    {
        public double WorstRating { get; }
        public DateTimeOffset WorstEventDate { get; }
        public Comment WorstEventComment { get; }
        public Guid WorstEventId { get; } 
    }
}