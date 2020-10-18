using System;

namespace ItHappened.Domain
{
    public interface IEventRepository
    {
        void AddEvent(Event newEvent);
        Event LoadEvent(Guid eventId);
        void DeleteEvent(Guid eventId);
    }
}