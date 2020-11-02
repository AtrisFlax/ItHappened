using System;
using System.ComponentModel.DataAnnotations;

namespace ItHappened.Infrastructure.Mappers
{
    public abstract class FactDto
    {
        public Guid Id { get; set; } //primary key
        public string FactName { get; set; }
        public string Description { get; set; }
        public double Priority { get; set; }

        public Guid UserId { get; set; }
        public Guid? TrackerId { get; set; } //for general facts TrackerId == NULL
    }
}