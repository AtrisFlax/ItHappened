using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using Serilog;
using Usage;

namespace Demo
{
    internal class Demo
    {
        private static readonly Random Gen = new Random();

        private static void Main(string[] args)
        {
            //create logger
            CreateLogger();

            //create entry point
            var compositionRoot = CompositionRoot.Create();

            //user service
            var userService = compositionRoot.UserService;

            //create users
            var userId = userService.CreateUser("Login_User1");
            var userId1 = userService.CreateUser("Login_User2");
            var userId2 = userService.CreateUser("Login_User3");
            var userId3 = userService.CreateUser("Login_User4");

            //event tracker service
            var eventTrackerService = compositionRoot.EventTrackerService;

            //create tracker
            var trackerId = eventTrackerService.CreateTracker(
                userId,
                "Tracker Name",
                true,
                true,
                "Kg",
                true,
                true,
                true);

            //add events to tracker
            const int numEvents = 110;
            var events = CreateEvents(userId, numEvents);
            foreach (var @event in events)
            {
                eventTrackerService.AddEventToTracker(userId, trackerId, @event);
            }

            //get all events from tracker 
            var allEvents = eventTrackerService.GetAllEventsFromTracker(trackerId, userId);

            //delete first 30
            allEvents.Do(x =>
            {
                for (var i = 0; i < 30; i++)
                {
                    var randomIndex = Gen.Next(0, x.Count);
                    eventTrackerService.RemoveEventFromTracker(userId, trackerId, x[randomIndex].Id);
                }
            });
            allEvents = eventTrackerService.GetAllEventsFromTracker(trackerId, userId);

            //edit event from allEvents list 
            const int targetEventNum = 15;
            allEvents.Do(x =>
            {
                var eventIdForEdit = x[targetEventNum].Id; 
                var eventForReplace = CreateEvent(userId, eventIdForEdit, "Tittle of Replaced Event", RandomDay());
                eventTrackerService.EditEventInTracker(userId, trackerId, eventIdForEdit, eventForReplace); 
            });
            
            
            //filtration
            var userIdFiltering = userService.CreateUser("Login_User_Filter");
            var trackerId1 = eventTrackerService.CreateTracker(
                userIdFiltering,
                "Tracker Name",
                true,
                true,
                "Kg",
                true,
                true,
                true);
            var eventsForFiltering = CreateEventsForFiltering(userIdFiltering).ToList();
            foreach (var @event in eventsForFiltering)
            {
                eventTrackerService.AddEventToTracker(userIdFiltering, trackerId1, @event);
            }

            var fromTwoMonthAgo = DateTimeOffset.Now.AddDays(-31 * 4);
            var toOneMonthAgo = DateTimeOffset.Now.AddDays(-31 * 2);
            var filtratedEvents =
                eventTrackerService.GetEventsFiltratedByTime(userIdFiltering, trackerId1, fromTwoMonthAgo,
                    toOneMonthAgo);

            //customization
            var customUserId = userService.CreateUser("CustomUser");
            var customTrackerId = eventTrackerService.CreateTracker(
                customUserId,
                "Tracker Name",
                false,
                false,
                "",
                true,
                false,
                false);
            var eventsWithRating = CreateEventsWithRating(customUserId, 50);
            foreach (var @event in eventsWithRating)
            {
                eventTrackerService.AddEventToTracker(customUserId, customTrackerId, @event);
            }

            var allEventsFromCustomTracker = eventTrackerService.GetAllEventsFromTracker(customTrackerId, customUserId);

            //statistic
            var userIdStatistic = userService.CreateUser("Login_User_Stats");
            var statisticService = compositionRoot.StatisticsService;
            var trackerIdStat1 = eventTrackerService.CreateTracker(
                userIdStatistic,
                "Tracker Name stat1",
                true,
                true,
                "Kg",
                true,
                true,
                true);
            var trackerIdStat2 = eventTrackerService.CreateTracker(
                userIdStatistic,
                "Tracker Name stat2",
                true,
                true,
                "Kg",
                true,
                true,
                true);
            var trackerIdStat3 = eventTrackerService.CreateTracker(
                userIdStatistic,
                "Tracker Name stat3",
                true,
                true,
                "Kg",
                true,
                true,
                true);
            const int numEventsStats = 500;
            var eventsStat1 = CreateEvents(userId, Gen.Next() % 100 + numEventsStats);
            var eventsStat2 = CreateEvents(userId, Gen.Next() % 100 + numEventsStats);
            var eventsStat3 = CreateEvents(userId, Gen.Next() % 100 + numEventsStats);
            foreach (var @event in eventsStat1)
            {
                eventTrackerService.AddEventToTracker(userIdStatistic, trackerIdStat1, @event);
            }

            foreach (var @event in eventsStat2)
            {
                eventTrackerService.AddEventToTracker(userIdStatistic, trackerIdStat2, @event);
            }

            foreach (var @event in eventsStat3)
            {
                eventTrackerService.AddEventToTracker(userIdStatistic, trackerIdStat3, @event);
            }

            //calculate 
            var allFacts = statisticService.GetFacts(userIdStatistic);
            var generalFacts= statisticService.GetGeneralFacts(userIdStatistic);
            var specificFacts = statisticService.GetSpecificFacts(userIdStatistic);
        }

        private static IEnumerable<Event> CreateEventsWithRating(Guid userId, in int numEvents)
        {
            var events = new List<Event>();
            for (var i = 0; i < numEvents; i++)
                events.Add(CreateEventWithOnlyRating(userId, $"Event with rating {i.ToString()}", RandomDay()));
            return events;
        }

        private static IEnumerable<Event> CreateEventsForFiltering(Guid userId)
        {
            return new List<Event>
            {
                CreateEvent(userId, "Filter Event1", DateTimeOffset.Now.AddDays(-31 * 7)),
                CreateEvent(userId, "Filter Event2", DateTimeOffset.Now.AddDays(-31 * 6)),
                CreateEvent(userId, "Filter Event3", DateTimeOffset.Now.AddDays(-31 * 5)),
                CreateEvent(userId, "Filter Event4", DateTimeOffset.Now.AddDays(-31 * 4)),
                CreateEvent(userId, "Not filtered Event5", DateTimeOffset.Now.AddDays(-31 * 3)), //filter
                CreateEvent(userId, "Not filtered Event6", DateTimeOffset.Now.AddDays(-31 * 2)), //filter
                CreateEvent(userId, "Filter Event7", DateTimeOffset.Now.AddDays(-31 * 1)),
                CreateEvent(userId, "Filter Event8", DateTimeOffset.Now.AddDays(-31 * 0))
            };
        }

        private static IEnumerable<Event> CreateEvents(Guid userId, int numEvents)
        {
            var events = new List<Event>();
            for (var i = 0; i < numEvents; i++)
            {
                events.Add(CreateEvent(userId, i.ToString(), RandomDay()));
            }
            return events;
        }

        private static Event CreateEvent(Guid userId, string title, DateTimeOffset happensDate)
        {
            return EventBuilder
                .Event(Guid.NewGuid(), userId, happensDate, $"Event {title}")
                .WithPhoto(new Photo(GetByteArray(Gen.Next() % 10 + 10)))
                .WithScale(Gen.NextDouble())
                .WithRating(Gen.NextDouble())
                .WithGeoTag(new GeoTag(Gen.NextDouble(), Gen.NextDouble()))
                .WithComment($"Comment of {title}")
                .Build();
        }

        private static Event CreateEvent(Guid userId, Guid eventId, string title, DateTimeOffset happensDate)
        {
            return EventBuilder
                .Event(eventId, userId, happensDate, $"Event {title}")
                .WithPhoto(new Photo(GetByteArray(Gen.Next() % 10 + 10)))
                .WithScale(Gen.NextDouble())
                .WithRating(Gen.NextDouble())
                .WithGeoTag(new GeoTag(Gen.NextDouble(), Gen.NextDouble()))
                .WithComment($"Comment of {title}")
                .Build();
        }

        private static Event CreateEventWithOnlyRating(Guid userId, string title, DateTimeOffset happensDate)
        {
            return EventBuilder
                .Event(Guid.NewGuid(), userId, happensDate, $"Event {title}")
                .WithRating(Gen.NextDouble())
                .Build();
        }

        private static void CreateLogger()
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .MinimumLevel.Verbose()
                .CreateLogger();
            Log.Logger = logger;
        }

        private static DateTime RandomDay()
        {
            var start = new DateTime(1995, 1, 1);
            var range = (DateTime.Today - start).Days;
            return start.AddDays(Gen.Next(range));
        }

        private static byte[] GetByteArray(int size)
        {
            var b = new byte[size];
            Gen.NextBytes(b);
            return b;
        }
    }
}