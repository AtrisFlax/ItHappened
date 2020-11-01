using System;
using ItHappened.Domain;
using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.Dto
{
    public class BestRatingEventFactDto : FactDto
    {
        public double BestRating { get; }
        public DateTimeOffset BestEventDate { get; }
        public Comment BestEventComment { get; }
    }
}