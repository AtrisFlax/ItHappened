using System;

namespace ItHappend.Domain
{
    internal interface IEventRepository
    {
        void TrySaveEvent(Event newEvent);
        Event TryLoadEvent(Guid eventId);
        void TryDeleteEvent(Guid eventId);
    }
}