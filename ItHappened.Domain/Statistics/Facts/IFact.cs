namespace ItHappened.Domain.Statistics
{
    public interface IFact
    {
        string FactName { get; }
        string Description { get; }
        double Priority { get; }
    }
}