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
            _commentFilter = new CommentFilter("regex", @"\A[з]");
        }

        [Test]
        public void FilterEventsWhichCommentStartsWithTheSameLetter_ReturnCollectionOfEvents()
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
            Assert.AreEqual(3, filteredEvents.Count());
        }
    }
}