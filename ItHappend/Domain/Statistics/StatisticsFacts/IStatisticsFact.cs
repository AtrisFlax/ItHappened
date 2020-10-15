namespace ItHappend.Domain.Statistics.StatisticsFacts
{
    public interface IStatisticsFact
    {
        string Description { get; }
        double Priority { get; }
    }
}