namespace ItHappend.Domain.StatisticsFacts
{
    public interface IStatisticsFact
    {
        string Description { get; }
        double Priority { get; }
    }
}