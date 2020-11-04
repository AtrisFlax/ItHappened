using System;
using System.Linq;
using ItHappened.Application.Errors;
using ItHappened.Application.Services.TrackerService;
using ItHappened.Domain;
using ItHappened.Infrastructure.Repositories;
using ItHappened.UnitTests.StatisticsCalculatorsTests;
using LanguageExt;
using NUnit.Framework;

namespace ItHappened.UnitTests.ServicesTests
{
    public class TrackerServiceTests
    {
        private EventTracker _tracker;
        private ITrackerService _trackerService;
        private ITrackerRepository _trackerRepository;

        [SetUp]
        public void Init()
        {
            _trackerRepository = new TrackerRepository();
            _trackerService = new TrackerService(_trackerRepository);
            _tracker = TestingMethods.CreateTrackerWithDefaultCustomization(Guid.NewGuid());
        }

        [Test]
        public void CreateTracker_CreatesTrackerWithRequiredParameters()
        {
            var creatorId = Guid.NewGuid();
            var trackerId = _trackerService.CreateEventTracker(creatorId, "tracker",
                new TrackerCustomizationSettings(
                    true,
                    true,
                    Option<string>.Some("meter"),
                    false,
                    true,
                    false,
                    false));

            var trackerFromRepository = _trackerRepository.LoadTracker(trackerId);

            var actualMeasurementUnit = trackerFromRepository.CustomizationSettings.ScaleMeasurementUnit.Match(
                Some: value => value,
                None: string.Empty);

            Assert.AreEqual(creatorId, trackerFromRepository.CreatorId);
            Assert.AreEqual("meter", actualMeasurementUnit);
            Assert.True(trackerFromRepository.CustomizationSettings.IsPhotoRequired);
            Assert.False(trackerFromRepository.CustomizationSettings.IsRatingRequired);
            Assert.True(trackerFromRepository.CustomizationSettings.IsGeotagRequired);
            Assert.False(trackerFromRepository.CustomizationSettings.IsCommentRequired);
            Assert.True(trackerFromRepository.CustomizationSettings.IsScaleRequired);
        }

        [Test]
        public void TwoTrackersWithSameName()
        {
            var creatorId = Guid.NewGuid();
            var sameTrackerName = "trackerName";
            var tracker1Id = _trackerService.CreateEventTracker(creatorId, sameTrackerName,
                new TrackerCustomizationSettings(
                    true,
                    true,
                    Option<string>.Some("meter"),
                    false,
                    true,
                    false,
                    false));


            Assert.Throws<DuplicateTrackerNameException>(() =>
                {
                    var tracker2Id = _trackerService.CreateEventTracker(creatorId, sameTrackerName,
                        new TrackerCustomizationSettings(
                            true,
                            true,
                            Option<string>.Some("meter"),
                            false,
                            true,
                            false,
                            false));
                }
            );  
        }

        [Test]
        public void GetTrackerWhenNoSuchTrackerInRepository_ThrowsException()
        {
            Assert.Throws<TrackerNotFoundException>(() =>
                _trackerService.GetEventTracker(Guid.NewGuid(), Guid.NewGuid()));
        }

        [Test]
        public void GetTrackerWhenUserAsksNotHisTracker_ThrowsException()
        {
            _trackerRepository.SaveTracker(_tracker);

            Assert.Throws<NoPermissionsForTrackerException>(() =>
                _trackerService.GetEventTracker(Guid.NewGuid(), _tracker.Id));
        }

        [Test]
        public void GetTrackerGoodCase_ReturnsRequiredTracker()
        {
            _trackerRepository.SaveTracker(_tracker);
            var tracker = _trackerService.GetEventTracker(_tracker.CreatorId, _tracker.Id);

            Assert.AreEqual(tracker.GetHashCode(), _tracker.GetHashCode());
        }

        [Test]
        public void GetAllUserTrackersWhenUserHasNoTrackers_ReturnsEmptyCollection()
        {
            const int expected = 0;

            var actual = _trackerRepository.LoadAllUserTrackers(Guid.NewGuid()).Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAllUserTrackers_ReturnsUserTrackers()
        {
            var userId = Guid.NewGuid();
            var tracker1 = TestingMethods.CreateTrackerWithDefaultCustomization(userId);
            var tracker2 = TestingMethods.CreateTrackerWithDefaultCustomization(userId);
            _trackerRepository.SaveTracker(tracker1);
            _trackerRepository.SaveTracker(tracker2);
            const int expected = 2;

            var actual = _trackerRepository.LoadAllUserTrackers(userId).Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EditEventTrackerWhenNoSuchTrackerInRepository_ThrowsException()
        {
            Assert.Throws<TrackerNotFoundException>(() => _trackerService.EditEventTracker(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "EditedTracker",
                new TrackerCustomizationSettings()));
        }

        [Test]
        public void EditEventTrackerWhenUserAsksNotHisTracker_ThrowsException()
        {
            _trackerRepository.SaveTracker(_tracker);
            Assert.Throws<NoPermissionsForTrackerException>(() => _trackerService.EditEventTracker(
                Guid.NewGuid(),
                _tracker.Id,
                "EditedTracker",
                new TrackerCustomizationSettings()));
        }

        [Test]
        public void EditEventTrackerGoodCase_EditedTrackerSavedInRepository()
        {
            _trackerRepository.SaveTracker(_tracker);

            var newName = "New Tracker Name";
            var editedCustomization = new TrackerCustomizationSettings();
            _trackerService.EditEventTracker(_tracker.CreatorId, _tracker.Id, newName,
                editedCustomization);

            var editedTrackerFromRepository = _trackerRepository.LoadTracker(_tracker.Id);

            Assert.AreEqual(newName, editedTrackerFromRepository.Name);
            Assert.AreEqual(editedCustomization.GetHashCode(),
                editedTrackerFromRepository.CustomizationSettings.GetHashCode());
        }

        [Test]
        public void DeleteTrackerWhenNoSuchTrackerInRepository_ThrowsException()
        {
            Assert.Throws<TrackerNotFoundException>(() =>
                _trackerService.DeleteEventTracker(Guid.NewGuid(), Guid.NewGuid()));
        }

        [Test]
        public void DeleteTrackerWhenUserAsksNotHisTracker_ThrowsException()
        {
            _trackerRepository.SaveTracker(_tracker);

            Assert.Throws<NoPermissionsForTrackerException>(() =>
                _trackerService.DeleteEventTracker(Guid.NewGuid(), _tracker.Id));
        }

        [Test]
        public void DeleteTrackerGoodCase_TrackerRemovedFromRepository()
        {
            _trackerRepository.SaveTracker(_tracker);

            _trackerService.DeleteEventTracker(_tracker.CreatorId, _tracker.Id);

            Assert.False(_trackerRepository.IsContainTracker(_tracker.Id));
        }
    }
}