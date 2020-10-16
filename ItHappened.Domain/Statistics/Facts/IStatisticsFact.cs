namespace ItHappened.Domain.Statistics.Facts
{
    public interface IStatisticsFact
    {
        string FactName { get; }
        string Description { get; }
        double Priority { get; }
    }
}