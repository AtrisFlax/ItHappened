// using System;
// using ItHappened.Domain;
// using ItHappened.Infrastructure.Repositories;
// using NUnit.Framework;
//
// namespace ItHappened.UnitTests
// {
//     public class EventTrackerTests
//     {
//         private EventTracker _eventTracker;
//
//         [SetUp]
//         public void Init()
//         {
//             _eventTracker = CreateEventTracker();
//
//         }
//
//         [Test]
//         public void IsTrackerAndEventCustomizationsMatchWhenCustomisationDiffers_ReturnFalse()
//         {
//             var eventToCompare = CreateEvent(Guid.NewGuid(), _eventTracker.Id);
//             eventToCompare.Comment = new Comment("comment");
//
//             var isMatch = _eventTracker.IsTrackerAndEventCustomizationsMatch(eventToCompare);
//             
//             Assert.False(isMatch);
//             Assert.AreNotEqual(_eventTracker.HasComment, eventToCompare.Comment.IsSome);
//             Assert.AreEqual(_eventTracker.HasPhoto, eventToCompare.Photo.IsSome);
//             Assert.AreEqual(_eventTracker.HasGeoTag, eventToCompare.GeoTag.IsSome);
//             Assert.AreEqual(_eventTracker.HasRating, eventToCompare.Rating.IsSome);
//             Assert.AreEqual(_eventTracker.HasScale, eventToCompare.Scale.IsSome);
//         }
//         
//         [Test]
//         public void IsTrackerAndEventCustomizationsMatchWhenCustomisationTheSame_ReturnTrue()
//         {
//             var trackerWithCommentAndGeoTag = CreateEventTrackerWithCommentAndGeoTag();
//             var eventToCompare = CreateEvent(Guid.NewGuid(), _eventTracker.Id);
//             eventToCompare.Comment = new Comment("comment");
//             eventToCompare.GeoTag = new GeoTag(54.1, 63.2);
//
//             var isMatch = trackerWithCommentAndGeoTag.IsTrackerAndEventCustomizationsMatch(eventToCompare);
//             
//             Assert.True(isMatch);
//             Assert.AreEqual(trackerWithCommentAndGeoTag.HasComment, eventToCompare.Comment.IsSome);
//             Assert.AreEqual(trackerWithCommentAndGeoTag.HasPhoto, eventToCompare.Photo.IsSome);
//             Assert.AreEqual(trackerWithCommentAndGeoTag.HasGeoTag, eventToCompare.GeoTag.IsSome);
//             Assert.AreEqual(trackerWithCommentAndGeoTag.HasRating, eventToCompare.Rating.IsSome);
//             Assert.AreEqual(trackerWithCommentAndGeoTag.HasScale, eventToCompare.Scale.IsSome);
//         }
//         
//         private EventTracker CreateEventTracker()
//         {
//             var tracker = EventTrackerBuilder
//                 .Tracker(Guid.NewGuid(), Guid.NewGuid(), "tracker")
//                 .Build();
//
//             return tracker;
//         }
//         
//         private EventTracker CreateEventTrackerWithCommentAndGeoTag()
//         {
//             var tracker = EventTrackerBuilder
//                 .Tracker(Guid.NewGuid(), Guid.NewGuid(), "tracker")
//                 .WithComment()
//                 .WithGeoTag()
//                 .Build();
//
//             return tracker;
//         }
//
//         private Event CreateEvent(Guid creatorId, Guid trackerId)
//         {
//             return EventBuilder
//                 .Event(Guid.NewGuid(), creatorId, trackerId, DateTimeOffset.Now, "event: " + $"{creatorId}")
//                 .Build();
//         }
//     }
// }