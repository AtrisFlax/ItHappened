namespace ItHappened.Domain.Statistics
{
    public interface ITrackerFact
    {
        string FactName { get; }
        string Description { get; }
        double Priority { get; }
    }
}