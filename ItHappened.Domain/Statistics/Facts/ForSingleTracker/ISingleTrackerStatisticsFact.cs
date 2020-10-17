namespace ItHappened.Domain.Statistics.Facts.ForSingleTracker
{
    public interface ISingleTrackerStatisticsFact : IStatisticsFact
    {
        EventTracker TargetEventTracker { get; }
    }
}