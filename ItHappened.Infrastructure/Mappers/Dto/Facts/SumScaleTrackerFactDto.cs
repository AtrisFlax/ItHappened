using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.Dto
{
    public class SumScaleTrackerFactDto : FactDto
    {
        public double SumValue { get; }
        public string MeasurementUnit { get; }
    }
}