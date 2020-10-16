namespace ItHappened.Domain.Statistics.Facts
{
    public interface IStatisticsFact
    {
        string Description { get; }
        double Priority { get; }
    }
}