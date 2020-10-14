using System;

namespace ItHappend.Domain
{
    public interface IEventRepository
    {
        void SaveEvent(Event newEvent);
        Event LoadEvent(Guid eventId);
        void DeleteEvent(Guid eventId);
    }
}