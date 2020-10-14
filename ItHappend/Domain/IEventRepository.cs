using System;

namespace ItHappend.Domain
{
    public interface IEventRepository
    {
        void SaveEvent(Event newEvent);
        Event TryLoadEvent(Guid eventId);
        void DeleteEvent(Guid eventId);
    }
}