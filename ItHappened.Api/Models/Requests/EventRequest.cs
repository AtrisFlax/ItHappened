using System;
using LanguageExt;

namespace ItHappened.Api.Models.Requests
{
    public class EventRequest
    {
        public DateTimeOffset HappensDate { get; set; }
        public EventCustomParametersRequest CustomParameters { get; set; }
    }
}