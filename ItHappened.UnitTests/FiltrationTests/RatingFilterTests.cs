// using System;
// using System.Collections.Generic;
// using ItHappened.Domain;
// using NUnit.Framework;
//
// namespace ItHappened.UnitTests.FiltrationTests
// {
//     public class RatingFilterTests
//     {
//         private IEventsFilter _ratingFilter;
//         [SetUp]
//         public void Init()
//         {
//             _ratingFilter = new RatingFilter("rating",  5.5, 8.6);
//         }
//         
//         [Test]
//         public void FilterEvents_ReturnFilteredCollection()
//         {
//             var events = CreateEvents(10);
//             events[0].Rating = 5.5;
//             events[1].Rating = 8.6;
//             events[2].Rating = 6;
//             events[3].Rating = 5.499;
//             events[4].Rating = 8.601;
//             const int expected = 3;
//             
//             var filteredEvents = _ratingFilter.Filter(events);
//             
//             Assert.AreEqual(expected, filteredEvents.Count);
//         }
//         
//         [Test]
//         public void FilterEventsWithoutRating_ReturnEmptyCollection()
//         {
//             var events = CreateEventsWithoutRating(10);
//
//             const int expected = 0;
//             
//             var filteredEvents = _ratingFilter.Filter(events);
//             
//             Assert.AreEqual(expected, filteredEvents.Count);
//         }
//         
//         private List<Event> CreateEvents(int quantity)
//         {
//             var events = new List<Event>();
//             for (var i = 0; i < quantity; i++)
//                 events.Add(EventBuilder
//                     .Event(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now, "tittle")
//                     .WithRating(1)
//                     .Build());
//             return events;
//         }
//         
//         private List<Event> CreateEventsWithoutRating(int quantity)
//         {
//             var events = new List<Event>();
//             for (var i = 0; i < quantity; i++)
//                 events.Add(EventBuilder
//                     .Event(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now, "tittle")
//                     .Build());
//             return events;
//         }
//     }
// }