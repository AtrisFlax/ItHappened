namespace ItHappend.Domain
{
    public enum StatisticServiceStatusCodes
    {
        Created,
        ApplicabilityFunctionDoesNotCompute,
        NoRatingInEventTrackers,
        NotEnoughEventsInEventTracker,
        EventTrackerHasNoRating,
        EventTrackerHasNotEnoughEvents,
        NotAllEventInTrackerHasRating
    }
}