namespace ItHappened.Application.Services.EventTrackerService
{
    public enum EventTrackerServiceStatusCodes
    {
        Ok,
        WrongInitiatorId,
        TrackerDontExist,
        WrongEventCreatorId,
        EventDontExist,
        EventAlreadyExist,
        WrongEventCustomisation
    }
}