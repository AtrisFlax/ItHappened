﻿namespace ItHappened.Api.Contracts
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Trackers
        {
            public const string Create = Base + "/trackers";

            public const string Get = Base + "/trackers/{trackerId}";

            public const string GetAll = Base + "/trackers";

            public const string Update = Base + "/trackers/{trackerId}";

            public const string Delete = Base + "/trackers/{trackerId}";
            
        }
        
        public static class Events
        {
            public const string Create = "/trackers/{trackerId}/events";

            public const string Get = "/trackers/{trackerId}/events/{eventId}";

            public const string GetAll = "/trackers/{trackerId}/events";
            
            public const string Update = "/trackers/{trackerId}/events/{eventId}";

            public const string Delete = "/trackers/{trackerId}/events/{eventId}";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            
            public const string Register = Base + "/identity/register";
            
            public const string Refresh = Base + "/identity/refresh";
        }
    }
}