using System;

namespace ItHappend.Domain
{
    interface IEventTrackerRepository
    {
        UserInfo GetUserInfo(Guid userId);
    }
}