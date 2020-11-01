using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.Dto
{
    public class MostFrequentEventTrackersFactDto : FactDto
    {
        public string TrackingName { get; }
        public double EventsPeriod { get; }
    }
}