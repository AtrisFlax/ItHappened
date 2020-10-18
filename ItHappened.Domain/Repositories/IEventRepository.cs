using System;

namespace ItHappened.Domain
{
    public interface IEventRepository
    {
        void AddEvent(Event newEvent);
        Event TryLoadEvent(Guid eventId);
        void DeleteEvent(Guid eventId);
    }
}