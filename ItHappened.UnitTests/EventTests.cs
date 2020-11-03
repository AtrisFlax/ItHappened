using System;
using ItHappened.Domain;
using LanguageExt;
using NUnit.Framework;

namespace ItHappened.UnitTests
{
    public class EventTests
    {
        private Guid _creatorId;
        private DateTimeOffset _date;
        private Guid _eventId;
        private GeoTag _geoTag;
        private Photo _photo;
        private double _rating;
        private double _scale;
        private Comment _textComment;
        private EventTracker _tracker;
        private string _title;

        [SetUp]
        public void Init()
        {
            _eventId = Guid.NewGuid();
            _creatorId = Guid.NewGuid();
            _date = DateTimeOffset.Now;
            _textComment = new Comment("Comment For Event");
            _scale = 15;
            _photo = new Photo(new byte[] {0x1, 0x2, 0x3});
            _rating = 299.0;
            _geoTag = new GeoTag(55.790514, 37.584822);
            _title = "Tracker name";
            _tracker = new EventTracker(Guid.NewGuid(), Guid.NewGuid(), _title,
                new TrackerCustomizationSettings());
        }

        [Test]
        public void CreationEventWithRequiredParameters()
        {
            //arrange

            //act
            var @event = new Event(_eventId, _creatorId, _tracker.Id, _date, "Event title",
                new EventCustomParameters());

            //assert
            Assert.AreEqual(_eventId, @event.Id);
            Assert.AreEqual(_creatorId, @event.CreatorId);
            Assert.AreEqual(_date, @event.HappensDate);
            Assert.AreEqual(_title, @event.Title);

            Assert.IsTrue(@event.CustomizationsParameters.Comment.IsNone);
            Assert.IsTrue(@event.CustomizationsParameters.Scale.IsNone);
            Assert.IsTrue(@event.CustomizationsParameters.Photo.IsNone);
            Assert.IsTrue(@event.CustomizationsParameters.Rating.IsNone);
            Assert.IsTrue(@event.CustomizationsParameters.GeoTag.IsNone);
        }

        [Test]
        public void CreationEventWithAllOptionalParameters()
        {
            //arrange

            //act
            var @event = new Event(_eventId,
                _creatorId,
                _tracker.Id,
                _date, "Event title",
                new EventCustomParameters(
                    Option<Photo>.Some(_photo),
                    Option<double>.Some(_scale),
                    Option<double>.Some(_rating),
                    Option<GeoTag>.Some(_geoTag),
                    Option<Comment>.Some(_textComment))
            );

            //assert
            Assert.AreEqual(_eventId, @event.Id);
            Assert.AreEqual(_creatorId, @event.CreatorId);
            Assert.AreEqual(_date, @event.HappensDate);

            Assert.IsTrue(@event.CustomizationsParameters.Comment.IsSome);
            Assert.IsTrue(@event.CustomizationsParameters.Scale.IsSome);
            Assert.IsTrue(@event.CustomizationsParameters.Photo.IsSome);
            Assert.IsTrue(@event.CustomizationsParameters.Rating.IsSome);
            Assert.IsTrue(@event.CustomizationsParameters.GeoTag.IsSome);

            Assert.IsTrue(@event.CustomizationsParameters.Comment == _textComment);
            Assert.IsTrue(@event.CustomizationsParameters.Scale == _scale);
            Assert.IsTrue(@event.CustomizationsParameters.Photo == _photo);
            Assert.IsTrue(@event.CustomizationsParameters.Rating == _rating);
            Assert.IsTrue(@event.CustomizationsParameters.GeoTag == _geoTag);
        }

        [Test]
        public void CreationEventAllParametersButSkipPhotoParameter()
        {
            //arrange

            //act
            var @event = new Event(_eventId,
                _creatorId,
                _tracker.Id,
                _date, "Event title",
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.Some(_scale),
                    Option<double>.Some(_rating),
                    Option<GeoTag>.Some(_geoTag),
                    Option<Comment>.Some(_textComment))
            );

            //assert
            Assert.AreEqual(_eventId, @event.Id);
            Assert.AreEqual(_creatorId, @event.CreatorId);
            Assert.AreEqual(_date, @event.HappensDate);

            Assert.IsTrue(@event.CustomizationsParameters.Comment.IsSome);
            Assert.IsTrue(@event.CustomizationsParameters.Scale.IsSome);
            Assert.IsTrue(@event.CustomizationsParameters.Photo.IsNone);
            Assert.IsTrue(@event.CustomizationsParameters.Rating.IsSome);
            Assert.IsTrue(@event.CustomizationsParameters.GeoTag.IsSome);

            Assert.IsTrue(@event.CustomizationsParameters.Comment == _textComment);
            Assert.IsTrue(@event.CustomizationsParameters.Scale == _scale);
            Assert.IsTrue(@event.CustomizationsParameters.Rating == _rating);
            Assert.IsTrue(@event.CustomizationsParameters.GeoTag == _geoTag);
        }
    }
}