using System;

namespace ItHappened.Bll.Domain.Repositories
{
    public interface IEventRepository
    {
        void SaveEvent(Event newEvent);
        Event TryLoadEvent(Guid eventId);
        void DeleteEvent(Guid eventId);
    }
}