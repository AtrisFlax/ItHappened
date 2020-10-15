using ItHappend.Domain;

namespace ItHappend.StatisticService
{
    public interface IMostFrequentEvent
    {
        (MostFrequentEvent, StatisticServiceStatusCodes) GetMostFrequentEventFact(EventTracker[] eventTrackers);
    }
}