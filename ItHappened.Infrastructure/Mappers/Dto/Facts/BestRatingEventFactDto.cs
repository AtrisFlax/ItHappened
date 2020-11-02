using System;
using ItHappened.Domain;
using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.Dto
{
    public class BestRatingEventFactDto : FactDto
    {
        public double BestRating { get; set; }
        public DateTimeOffset BestEventDate { get; set; }
        public Comment BestEventComment { get; set; }
    }
}