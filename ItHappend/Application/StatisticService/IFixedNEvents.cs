using ItHappend.Domain;
using ItHappend.Domain.Statistics;

namespace ItHappend.StatisticService
{
    public interface IFixedNEvents
    {
        (FixedNEventsFact, StatisticServiceStatusCodes) GetFixedNEventsFact();
    }
}