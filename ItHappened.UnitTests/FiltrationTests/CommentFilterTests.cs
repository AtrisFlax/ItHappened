// using System;
// using System.Collections.Generic;
// using ItHappened.Domain;
// using LanguageExt;
// using NUnit.Framework;
//
// namespace ItHappened.UnitTests.FiltrationTests
// {
//     public class CommentFilterTests
//     {
//         private IEventsFilter _commentFilter;
//         
//         [SetUp]
//         public void Init()
//         {
//             _commentFilter = new CommentFilter("regex",  @"\A[з]");
//         }
//
//         [Test]
//         public void FilterEventsWhichCommentStartsWithTheSameLetter_ReturnCollectionOfEvents()
//         {
//             var events = CreateEvents(10);
//             events[0].Comment = Option<Comment>.Some(new Comment("здравствуй!"));
//             events[1].Comment = Option<Comment>.Some(new Comment("Здравствуй!"));
//             events[2].Comment = Option<Comment>.Some(new Comment("ззз ззз З"));
//             const int expected = 3;
//             
//             var filteredEvents = _commentFilter.Filter(events);
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
//                     .WithComment("Привет!")
//                     .Build());
//             return events;
//         }
//     }
// }