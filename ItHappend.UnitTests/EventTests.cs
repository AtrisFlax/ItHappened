using System;
using ItHappend.Domain;
using ItHappend.Domain.EventCustomization;
using NUnit.Framework;

namespace ItHappend.UnitTests
{
    public class EventTests
    {
        private Guid _eventId;
        private Guid _creatorId;
        private DateTimeOffset _date;
        private string _title;
        private string _textComment;
        private double _scale;
        private Photo _photo;
        private double _rating;
        private GeoTag _geoTag;


        [SetUp]
        public void Init()
        {
            _eventId = Guid.NewGuid();
            _creatorId = Guid.NewGuid();
            _date = DateTimeOffset.Now;
            _title = "Title";
            _textComment = "Comment For Event";
            _scale = 15;
            _photo = new Photo( new byte[] {0x1, 0x2, 0x3});
            _rating = 299.0;
            _geoTag = new GeoTag(55.790514, 37.584822);
        }

        [Test]
        public void CreationEventWithRequiredParameters()
        {
            //arrange

            //act
            var newEvent = EventBuilder.Event(_eventId, _creatorId, _date, _title).Build();

            //assert
            Assert.AreEqual(_eventId, newEvent.Id);
            Assert.AreEqual(_creatorId, newEvent.CreatorId);
            Assert.AreEqual(_date, newEvent.HappensDate);
            Assert.AreEqual(_title, newEvent.Title);

            Assert.IsFalse(newEvent.Comment.IsSome);
            Assert.IsFalse(newEvent.Scale.IsSome);
            Assert.IsFalse(newEvent.Photo.IsSome);
            Assert.IsFalse(newEvent.Rating.IsSome);
            Assert.IsFalse(newEvent.GeoTag.IsSome);
        }

        [Test]
        public void CreationEventWithAllOptionalParameters()
        {
            //arrange

            //act
            var @event = EventBuilder
                .Event(_eventId, _creatorId, _date, _title)
                .WithComment(_textComment)
                .WithScale(_scale)
                .WithPhoto(_photo)
                .WithRating(_rating)
                .WithGeoTag(_geoTag)
                .Build();

            //assert
            Assert.AreEqual(_eventId, @event.Id);
            Assert.AreEqual(_creatorId, @event.CreatorId);
            Assert.AreEqual(_date, @event.HappensDate);
            Assert.AreEqual(_title, @event.Title);

            Assert.IsTrue(@event.Comment.IsSome);
            Assert.IsTrue(@event.Scale.IsSome);
            Assert.IsTrue(@event.Photo.IsSome);
            Assert.IsTrue(@event.Rating.IsSome);
            Assert.IsTrue(@event.GeoTag.IsSome);
            @event.Comment.Do(value => Assert.IsTrue(value.Text == _textComment));
            @event.Scale.Do(value => Assert.IsTrue(value == _scale));
            @event.Photo.Do(value => Assert.IsTrue(value == _photo));
            @event.Rating.Do(value => Assert.IsTrue(value == _rating));
            @event.GeoTag.Do(value => Assert.IsTrue(value == _geoTag));
        }


        [Test]
        public void CreationEventWithNullTitle()
        {
            //arrange
            var eventId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var date = DateTimeOffset.Now;
            const string title = null;

            //act

            //assert
            Assert.Throws<NullReferenceException>(() =>
                EventBuilder.Event(eventId, creatorId, date, title).Build()
            );
        }


        [Test]
        public void CreationEventAllParametersButSkipPhotoParameter()
        {
            //arrange
            var eventId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var date = DateTimeOffset.Now;
            const string title = "Title";
            const string textComment = "Comment For Event";
            const double scale = 15;
            const double rating = 299;
            var geoTag = new GeoTag(55.790514, 37.584822);


            //act
            var @event = EventBuilder
                .Event(eventId, creatorId, date, title)
                .WithComment(textComment)
                .WithScale(scale)
                .WithRating(rating)
                .WithGeoTag(geoTag)
                .Build();

            //assert
            Assert.AreEqual(eventId, @event.Id);
            Assert.AreEqual(creatorId, @event.CreatorId);
            Assert.AreEqual(date, @event.HappensDate);
            Assert.AreEqual(title, @event.Title);
            Assert.AreEqual(title, @event.Title);

            Assert.IsTrue(@event.Comment.IsSome);
            Assert.IsTrue(@event.Scale.IsSome);
            Assert.IsFalse(@event.Photo.IsSome);
            Assert.IsTrue(@event.Rating.IsSome);
            Assert.IsTrue(@event.GeoTag.IsSome);

            @event.Comment.Do(value => Assert.IsTrue(value.Text == _textComment));
            @event.Scale.Do(value => Assert.IsTrue(value == _scale));
            @event.Photo.Do(value => Assert.IsTrue(value == _photo));
            @event.Rating.Do(value => Assert.IsTrue(value == _rating));
            @event.GeoTag.Do(value => Assert.IsTrue(value == _geoTag));
        }
    }
}