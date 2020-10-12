using System;
using ItHappend.Domain;

namespace ItHappend
{
    internal interface IEventRepository
    {
        Guid SaveEvent(Event newEvent);
        Event LoadEvent(Guid eventId);
        void DeleteEvent(Guid eventId);
    }
}