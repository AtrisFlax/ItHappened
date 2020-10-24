namespace ItHappened.Domain.Statistics
{
    public interface IBackgroundStatisticGenerator
    {
        void UpdateAllUsersGeneralFacts();
        void UpdateAllUsersSpecificFacts();
    }
}