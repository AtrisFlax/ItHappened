using System;
using ItHappened.Domain;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;

namespace ItHappened.UnitTests
{
    public class EventTrackerTests
    {
        [Test]
        public void CreateTrackerAllCustomizations()
        {
            //arrange
            var trackerId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            const string scaleUnit = "kg";

            //act
            const string trackerName = "Tracker name";
            var tracker = new EventTracker(
                trackerId,
                userId,
                trackerName,
                new TrackerCustomizationSettings(true,
                    true,
                    scaleUnit,
                    true,
                    true,
                    true, true)
            );

            //assert
            Assert.AreEqual(trackerId, tracker.Id);
            Assert.AreEqual(userId, tracker.CreatorId);
            Assert.AreEqual(tracker.Name, trackerName);
            Assert.AreEqual(scaleUnit, tracker.CustomizationSettings.ScaleMeasurementUnit.ValueUnsafe());
            Assert.True( tracker.CustomizationSettings.IsPhotoRequired);
            Assert.True( tracker.CustomizationSettings.IsRatingRequired);
            Assert.True( tracker.CustomizationSettings.IsGeotagRequired);
            Assert.True( tracker.CustomizationSettings.IsCommentRequired);
            Assert.True( tracker.CustomizationSettings.IsScaleRequired);
        }

        [Test]
        public void CreateTrackerNoCustomizations()
        {
            //arrange
            var trackerId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            //act
            var tracker = new EventTracker(
                trackerId,
                userId,
                "Tracker name",
                new TrackerCustomizationSettings()
            );

            //assert
            Assert.AreEqual(trackerId, tracker.Id);
            Assert.AreEqual(userId, tracker.CreatorId);
            Assert.AreEqual(tracker.Name, "Tracker name");
            Assert.IsTrue(tracker.CustomizationSettings.ScaleMeasurementUnit.IsNone);
            Assert.IsFalse(tracker.CustomizationSettings.IsPhotoRequired);
            Assert.IsFalse(tracker.CustomizationSettings.IsRatingRequired);
            Assert.IsFalse(tracker.CustomizationSettings.IsGeotagRequired);
            Assert.IsFalse(tracker.CustomizationSettings.IsCommentRequired);
            Assert.IsFalse(tracker.CustomizationSettings.IsScaleRequired);
        }

        [Test]
        public void CreateTrackerWithScaleAndPhoto()
        {
            //arrange
            var trackerId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            const string scaleUnit = "kg";

            //act
            const string trackerName = "Tracker name";
            var tracker = new EventTracker(
                trackerId,
                userId,
                trackerName,
                new TrackerCustomizationSettings(true,
                    true,
                    scaleUnit,
                    false,
                    false,
                    true,
                    true)
            );

            //assert
            Assert.AreEqual(trackerId, tracker.Id);
            Assert.AreEqual(userId, tracker.CreatorId);
            Assert.AreEqual(tracker.Name, trackerName);
            Assert.AreEqual(scaleUnit, tracker.CustomizationSettings.ScaleMeasurementUnit.ValueUnsafe());
            Assert.IsTrue(tracker.CustomizationSettings.IsPhotoRequired);
            Assert.IsFalse(tracker.CustomizationSettings.IsRatingRequired);
            Assert.IsFalse(tracker.CustomizationSettings.IsGeotagRequired);
            Assert.IsTrue(tracker.CustomizationSettings.IsCommentRequired);
            Assert.IsTrue(tracker.CustomizationSettings.IsScaleRequired);
            Assert.IsTrue(tracker.CustomizationSettings.IsCustomizationRequired);
        }

        [Test]
        public void CustomizationsTrackerIsMatchingCustomizationsEventForceCustomizations_NotMatch()
        {
            //arrange
            var trackerId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            const string scaleUnit = "kg";
            const string trackerName = "Tracker name";
            var tracker = new EventTracker(
                trackerId,
                userId,
                trackerName,
                new TrackerCustomizationSettings(true,
                    true,
                    scaleUnit,
                    false,
                    false,
                    false,
                    true)
            );
            var @event = CreateEvent(tracker.Id);

            //act

            var isCustomizationsIsMatch = tracker.IsTrackerCustomizationAndEventCustomizationMatch(@event);
            // 

            //assert
            Assert.False(isCustomizationsIsMatch);
        }

        [Test]
        public void CustomizationsTrackerIsMatchingCustomizationsEventCustomizationsForce_Match()
        {
            //arrange
            var trackerId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            const string scaleUnit = "kg";
            const string trackerName = "Tracker name";
            var tracker = new EventTracker(
                trackerId,
                userId,
                trackerName,
                new TrackerCustomizationSettings(true,
                    true,
                    scaleUnit,
                    false,
                    false,
                    false,
                    true)
            );
            var @event = CreateMatchingEvent(tracker.Id);

            //act

            var isCustomizationsIsMatch = tracker.IsTrackerCustomizationAndEventCustomizationMatch(@event);
            // 

            //assert
            Assert.True(isCustomizationsIsMatch);
        }


        [Test]
        public void CustomizationsTrackerIsMatchingCustomizationsEventCustomizationsButNoForce_Match()
        {
            //arrange
            var trackerId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            const string scaleUnit = "kg";
            const string trackerName = "Tracker name";
            var tracker = new EventTracker(
                trackerId,
                userId,
                trackerName,
                new TrackerCustomizationSettings(true,
                    true,
                    scaleUnit,
                    false,
                    false,
                    false,
                    false)
            );
            var @event = CreateEvent(tracker.Id);

            //act

            var isCustomizationsIsMatch = tracker.IsTrackerCustomizationAndEventCustomizationMatch(@event);
            // 

            //assert
            Assert.True(isCustomizationsIsMatch);
        }


        private static Event CreateEvent(Guid trackerId)
        {
            var eventId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var date = DateTimeOffset.Now;
            var textComment = new Comment("Comment For Event");
            const int scale = 15;
            var photo = new Photo(new byte[] {0x1, 0x2, 0x3});
            var rating = 299.0;
            var geoTag = new GeoTag(55.790514, 37.584822);

            var @event = new Event(eventId,
                creatorId,
                trackerId,
                date, "Event title",
                new EventCustomParameters(
                    Option<Photo>.Some(photo),
                    Option<double>.Some(scale),
                    Option<double>.Some(rating),
                    Option<GeoTag>.Some(geoTag),
                    Option<Comment>.Some(textComment))
            );
            return @event;
        }

        private static Event CreateMatchingEvent(Guid trackerId)
        {
            var eventId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var date = DateTimeOffset.Now;
            const int scale = 15;
            var photo = new Photo(new byte[] {0x1, 0x2, 0x3});

            var @event = new Event(eventId,
                creatorId,
                trackerId,
                date, "Event title",
                new EventCustomParameters(
                    Option<Photo>.Some(photo),
                    Option<double>.Some(scale),
                    Option<double>.None,
                    Option<GeoTag>.None,
                    Option<Comment>.None)
            );
            return @event;
        }
    }
}