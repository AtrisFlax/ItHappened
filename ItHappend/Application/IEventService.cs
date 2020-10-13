using System;
using ItHappend.Domain;

namespace ItHappend
{
    internal interface IEventService
    {
        Event GetEvent(Guid eventId, Guid eventCreatorId);
        void CreateEvent(Event newEvent);
        void EditEvent(Guid eventId, Guid eventCreatorId, Event newEvent);
        void DeleteEvent(Guid eventId, Guid creatorId);
    }
}