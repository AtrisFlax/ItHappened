using ItHappend.Domain;
using ItHappend.Domain.Statistics;

namespace ItHappend.StatisticService
{
    public interface IMostFrequentEvent
    {
        (MostFrequentEvent, StatisticServiceStatusCodes) GetMostFrequentEventFact(EventTracker[] eventTrackers);
    }
}