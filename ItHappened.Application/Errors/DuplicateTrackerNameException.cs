using System.Collections.Generic;
using System.Net;

namespace ItHappened.Application.Errors
{
    public class DuplicateTrackerNameException : BusinessException
    {
        public DuplicateTrackerNameException(string name)
            : base(HttpStatusCode.BadRequest,
                "Cant create Tracker with already existing name ",
                new Dictionary<string, object> {{"Tracker name", name}})
        {
        }
    }
}