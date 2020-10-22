 using System;
 using System.Collections.Generic;
 using ItHappend.Domain.Statistics;
 using ItHappened.Domain;
 using ItHappened.Domain.Statistics;
 using ItHappened.Infrastructure.Repositories;
 using LanguageExt;
 using LanguageExt.UnsafeValueAccess;
 using NUnit.Framework;

 namespace ItHappened.UnitTests.StatisticsCalculatorsTests
 {
     public class WorstEventCalculatorTests
     {
         private const int InitialEventsNumber = 9;
         private Guid _creatorId;
         private List<Event> _events;
         private EventTracker _eventTracker;
         private WorstEventCalculator _worstEventCalculator;
         private IEventRepository _eventRepository;
         
         [SetUp]
         public void Init()
         {
             _eventRepository = new EventRepository();
             _creatorId = Guid.NewGuid();
             _eventTracker = CreateEventTracker();
             _events = CreateEvents(_creatorId, InitialEventsNumber);
             _eventRepository.AddRangeOfEvents(_events);
             _worstEventCalculator = new WorstEventCalculator(_eventRepository);
         }

         [Test]
         public void CalculateLessThanRequiredNumberEvents_ReturnNone()
         {
             //arrange
             _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
             _events[1].Rating = 1;
             _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
             var expected = Option<ISingleTrackerFact>.None;

             //act
             var actual = _worstEventCalculator.Calculate(_eventTracker);

             //assert
             Assert.AreEqual(expected, actual);
         }

         [Test]
         public void CalculateWithoutOldEnoughEvent_ReturnNone()
         {
             //arrange
             _events.Add(CreateEventWithoutComment(_creatorId));
             _events[1].Rating = 1;
             _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
             var expected = Option<ISingleTrackerFact>.None;

             //act
             var actual = _worstEventCalculator.Calculate(_eventTracker);

             //assert
             Assert.AreEqual(expected, actual);
         }

         [Test]
         public void CalculateWhenWorstEventHappenedLessThanWeekAgo_ReturnNone()
         {
             //arrange
             _events.Add(CreateEventWithoutComment(_creatorId));
             _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
             _events[1].Rating = 1;
             _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(6);
             var expected = Option<ISingleTrackerFact>.None;

             //act
             var actual = _worstEventCalculator.Calculate(_eventTracker);

             //assert
             Assert.AreEqual(expected, actual);
         }

         [Test]
         public void CalculateGoodCase_ReturnsFact()
         {
             //arrange
             _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
             var event10 = CreateEventWithoutComment(_creatorId);
             event10.Comment = new Comment("123");
             event10.Rating = 1;
             event10.HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
             _eventRepository.AddEvent(event10);

             //act
             var optionalWorstEventFact = 
                 _worstEventCalculator.Calculate(_eventTracker).ConvertTo<WorstEventFact>();
             var worstEventFact = optionalWorstEventFact.ValueUnsafe();

             //assert
             Assert.True(optionalWorstEventFact.IsSome);
             Assert.AreEqual(worstEventFact.Priority, 10 - event10.Rating.ValueUnsafe());
             Assert.AreEqual(worstEventFact.HappensDate, event10.HappensDate);
             Assert.AreEqual(worstEventFact.Comment.Text, event10.Comment.ValueUnsafe().Text);
             Assert.AreEqual(worstEventFact.FactName, "Худшее событие");
         }

         private Event CreateEventWithoutComment(Guid creatorId)
         {
             return EventBuilder
                 .Event(Guid.NewGuid(), creatorId, _eventTracker.Id, DateTimeOffset.Now, "tittle")
                 .WithRating(5)
                 .Build();
         }

         private List<Event> CreateEvents(Guid creatorId, int quantity)
         {
             var events = new List<Event>();
             for (var i = 0; i < quantity; i++)
                 events.Add(EventBuilder
                     .Event(Guid.NewGuid(), creatorId, _eventTracker.Id, DateTimeOffset.Now, "tittle")
                     .WithComment("comment")
                     .WithRating(5)
                     .Build());
             return events;
         }

         private EventTracker CreateEventTracker()
         {
             var tracker = EventTrackerBuilder
                 .Tracker(Guid.NewGuid(), Guid.NewGuid(), "tracker")
                 .Build();
             return tracker;
         }
     }
 }