using System;
using ItHappend.Domain;
using Microsoft.VisualBasic.FileIO;

namespace ItHappend
{
    interface IEventTrackerService
    {
        Event GetEvent(Guid eventId, Guid eventCreatorId);
        Guid CreateEvent(Guid eventId, string name, DateTimeOffset creationDate, decimal evaluation);
        void EditEvent(Guid eventId, string name, DateTimeOffset creationDate, decimal evaluation);
        void DeleteEvent(Guid eventId, Guid userId);
    }
}