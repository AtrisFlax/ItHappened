namespace ItHappened.Domain.Statistics
{
    public interface IStatisticsFact
    {
        string FactName { get; }
        string Description { get; }
        double Priority { get; }
    }
}