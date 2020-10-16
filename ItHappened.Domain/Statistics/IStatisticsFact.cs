namespace ItHappened.Domain.Statistics
{
    public interface IStatisticsFact
    {
        string Description { get; }
        double Priority { get; }
    }
}