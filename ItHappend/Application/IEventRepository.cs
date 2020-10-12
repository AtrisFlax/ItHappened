using System;
using ItHappend.Domain;

namespace ItHappend
{
    internal interface IEventRepository
    {
        //сделать во всех репозиториях аналогичные названия методов
        //либо Save... либо Ad...
        //думаю, что этот метод не должен ничего возвращать
        Guid SaveEvent(Event newEvent);
        //либо Load... либо Get...
        Event LoadEvent(Guid eventId);
        void DeleteEvent(Guid eventId);
    }
}