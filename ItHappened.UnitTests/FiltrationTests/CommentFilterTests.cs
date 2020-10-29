using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;

namespace ItHappened.UnitTests.FiltrationTests
{
    public class CommentFilterTests
    {
        private IEventsFilter _commentFilter;

        [SetUp]
        public void Init()
        {
            _commentFilter = new CommentFilter("фильтр", "дрА");
        }

        [Test]
        public void FilterEventsWithRussianComment_ReturnCollectionOfEvents()
        {
            //arrange
            var events = new List<Event>
            {
                CreateEventWithComment(Guid.NewGuid(), Guid.NewGuid(), "Было круто"),
                CreateEventWithComment(Guid.NewGuid(), Guid.NewGuid(), "Было круто"),
                CreateEventWithComment(Guid.NewGuid(), Guid.NewGuid(), "Было круто"),
                CreateEventWithComment(Guid.NewGuid(), Guid.NewGuid(), "Было круто"),
                CreateEventWithComment(Guid.NewGuid(), Guid.NewGuid(), "здравствуй!"),
                CreateEventWithComment(Guid.NewGuid(), Guid.NewGuid(), "Здравствуй!"),
                CreateEventWithComment(Guid.NewGuid(), Guid.NewGuid(), "ззз ззз З")
            };
            
            //act
            var filteredEvents = _commentFilter.Filter(events);

            //assert
            Assert.AreEqual(2, filteredEvents.Count());
        }
        
        [Test]
        public void FilterEventsWithEnglishComment_ReturnCollectionOfEvents()
        {
            var commentFilter= new CommentFilter("фильтр", "oL");
            //arrange
            var events = new List<Event>
            {
                CreateEventWithComment(Guid.NewGuid(), Guid.NewGuid(), "Was cool"),
                CreateEventWithComment(Guid.NewGuid(), Guid.NewGuid(), "Was cool"),
                CreateEventWithComment(Guid.NewGuid(), Guid.NewGuid(), "Было Was cool"),
                CreateEventWithComment(Guid.NewGuid(), Guid.NewGuid(), "Hello! Was cool"),
                CreateEventWithComment(Guid.NewGuid(), Guid.NewGuid(), "здорово! "),
                CreateEventWithComment(Guid.NewGuid(), Guid.NewGuid(), "ЗдоровО! "),
                CreateEventWithComment(Guid.NewGuid(), Guid.NewGuid(), "hello! ззз З ")
            };
            
            //act
            var filteredEvents = commentFilter.Filter(events);

            //assert
            Assert.AreEqual(4, filteredEvents.Count());
        }
    }
}