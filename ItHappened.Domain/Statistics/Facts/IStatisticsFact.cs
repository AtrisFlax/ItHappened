namespace ItHappened.Domain.Statistics.Facts
{
    public interface IStatisticsFact
    {
        string Type { get; }
        string Description { get; }
        double Priority { get; }
    }
}