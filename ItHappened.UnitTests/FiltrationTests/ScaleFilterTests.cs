// using System;
// using System.Collections.Generic;
// using ItHappened.Domain;
// using NUnit.Framework;
//
// namespace ItHappened.UnitTests.FiltrationTests
// {
//     public class ScaleFilterTests
//     {
//         private IEventsFilter _scaleFilter;
//         [SetUp]
//         public void Init()
//         {
//             _scaleFilter = new ScaleFilter("scale",  5.5, 8.6);
//         }
//         
//         [Test]
//         public void FilterEvents_ReturnFilteredCollection()
//         {
//             var events = CreateEvents(10);
//             events[0].Scale = 5.5;
//             events[1].Scale = 8.6;
//             events[2].Scale = 6;
//             events[3].Scale = 5.499;
//             events[4].Scale = 8.601;
//             const int expected = 3;
//             
//             var filteredEvents = _scaleFilter.Filter(events);
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
//                     .WithScale(1)
//                     .Build());
//             return events;
//         }
//     }
// }