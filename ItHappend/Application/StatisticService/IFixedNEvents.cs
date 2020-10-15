using ItHappend.Domain;

namespace ItHappend.StatisticService
{
    public interface IFixedNEvents
    {
        IFactStatistics GetFixedNEventsFact();
    }
}