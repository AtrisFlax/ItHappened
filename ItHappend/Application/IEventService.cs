using System;
using ItHappend.Domain;
using Microsoft.VisualBasic.FileIO;

namespace ItHappend
{
    internal interface IEventService
    {
        Event GetEvent(Guid eventId, Guid eventCreatorId);

        Guid CreateEvent(Guid eventId, Guid creatorId, string name, DateTimeOffset creationDate,
            decimal evaluation);

        void EditEvent(Guid eventId, Guid eventCreatorId, string newName, DateTimeOffset eventHappensDate,
            decimal evaluation);

        void DeleteEvent(Guid eventId, Guid creatorId);
    }
}