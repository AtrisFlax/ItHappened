using ItHappend.Domain;

namespace ItHappend.StatisticService
{
    public interface IFixedNEvents
    {
        (FixedNEventsFact, StatisticServiceStatusCodes) GetFixedNEventsFact();
    }
}