using System;
using System.Collections.Generic;
using ItHappened.Domain;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;

namespace ItHappened.UnitTests.FiltrationTests
{
    public class EventsFilterTests
    {
        private IEventsFilter _commentFilter;
        private IEventsFilter _correctGeoTagFilter;
        private IEventsFilter _ratingFilter;
        private IEventsFilter _dateTimeFilter;
        private IEventsFilter _scaleFilter;
        private IReadOnlyCollection<IEventsFilter> _filters;

        [SetUp]
        public void Init()
        {
            _commentFilter = new CommentFilter("regex",  @"\A[з]");
            _correctGeoTagFilter = new GeoTagFilter("geotag",  new GeoTag(10, 15), new GeoTag(20, 25));
            _ratingFilter = new RatingFilter("rating",  5.5, 8.6);
            var from = new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero);
            var to = new DateTimeOffset(2020, 10, 14, 18, 01, 00, TimeSpan.Zero);
            _dateTimeFilter = new DateTimeFilter("DateTime",  from, to);
            _scaleFilter = new ScaleFilter("scale",  5.5, 8.6);
            _filters = new[] {_commentFilter, _ratingFilter, _scaleFilter, _dateTimeFilter, _correctGeoTagFilter};
        }

        [Test]
        public void FilterCollection_ReturnCollectionThatMeetsAllFiltersConditions()
        {
            var events = CreateEvents(10);
            //Each one of these events won't pass its one parameter filter
            events[0].Comment = Option<Comment>.Some(new Comment("привет!"));
            events[1].Scale = 1;
            events[2].HappensDate = DateTimeOffset.Now;
            events[3].Rating = 1;
            events[4].GeoTag = new GeoTag(0, 0);
            const int expected = 5;

            var filteredEvents = EventsFilter.Filter(events, _filters);

            Assert.AreEqual(expected, filteredEvents.Count);
            Assert.AreEqual(filteredEvents[0].Rating.ValueUnsafe(), events[5].Rating.ValueUnsafe());
            Assert.AreEqual(filteredEvents[0].Scale.ValueUnsafe(), events[5].Scale.ValueUnsafe());
            Assert.AreEqual(filteredEvents[0].HappensDate, events[5].HappensDate);
            Assert.AreEqual(filteredEvents[0].GeoTag.ValueUnsafe().GpsLat, events[5].GeoTag.ValueUnsafe().GpsLat);
            Assert.AreEqual(filteredEvents[0].Comment.ValueUnsafe().Text, events[5].Comment.ValueUnsafe().Text);
        }
        
        //TODO: добавить тест, проверяющий, что порядок фильтров в коллекции влияет на результат фильтрации
        
        private List<Event> CreateEvents(int quantity)
        {
            var events = new List<Event>();
            for (var i = 0; i < quantity; i++)
                events.Add(EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                        new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero).ToLocalTime(),
                        "tittle")
                    .WithComment("здравствуй!")
                    .WithRating(5.6)
                    .WithScale(8)
                    .WithGeoTag(new GeoTag(11, 16))
                    .Build());
            return events;
        }
    }
}