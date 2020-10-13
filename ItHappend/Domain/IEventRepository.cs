using System;

namespace ItHappend.Domain
{
    internal interface IEventRepository
    {
        void SaveEvent(Event newEvent);
        Event LoadEvent(Guid eventId);
        void DeleteEvent(Guid eventId);
    }
}