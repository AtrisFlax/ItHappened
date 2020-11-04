using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Application.Errors;
using ItHappened.Domain;

namespace ItHappened.Application.Services.TrackerService
{
    public class TrackerService : ITrackerService
    {
        private readonly ITrackerRepository _trackerRepository;
        public TrackerService(ITrackerRepository trackerRepository)
        {
            _trackerRepository = trackerRepository;
        }
        
        public Guid CreateEventTracker(Guid creatorId, string trackerName, TrackerCustomizationSettings customizationSettings)
        {
            var tracker = new EventTracker(Guid.NewGuid(), creatorId, trackerName, customizationSettings);
            if (_trackerRepository.IsExistTrackerWithSameName(creatorId, trackerName))
            {
                throw new DuplicateTrackerNameException(trackerName);
            }
            _trackerRepository.SaveTracker(tracker);
            return tracker.Id;
        }

        public EventTracker GetEventTracker(Guid actorId, Guid trackerId)
        {
            if (!_trackerRepository.IsContainTracker(trackerId))
            {
                throw new TrackerNotFoundException(trackerId);
            }
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (actorId != tracker.CreatorId)
            {
                throw new NoPermissionsForTrackerException(actorId, trackerId);
            }
            return tracker;
        }
        
        public IReadOnlyCollection<EventTracker> GetEventTrackers(Guid actorId)
        {
            var trackers = _trackerRepository.LoadAllUserTrackers(actorId);
            return trackers.ToList();
        }
        
        public void EditEventTracker(Guid actorId,
            Guid trackerId,
            string name,
            TrackerCustomizationSettings customizationSettings)
        {
            if (!_trackerRepository.IsContainTracker(trackerId))
            {
                throw new TrackerNotFoundException(trackerId);
            }
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (actorId != tracker.CreatorId)
            {
                throw new NoPermissionsForTrackerException(actorId, trackerId);
            }

            if (_trackerRepository.IsExistTrackerWithSameName(actorId, name))
            {
                throw new DuplicateTrackerNameException(name);
            }
            _trackerRepository.SaveTracker(tracker);
            
            var updatedTracker = new EventTracker(tracker.Id, tracker.CreatorId, name, customizationSettings);
            _trackerRepository.UpdateTracker(updatedTracker);
        }

        public void DeleteEventTracker(Guid actorId, Guid trackerId)
        {
            if (!_trackerRepository.IsContainTracker(trackerId))
            {
                throw new TrackerNotFoundException(trackerId);
            }
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (actorId != tracker.CreatorId)
            {
                throw new NoPermissionsForTrackerException(actorId, trackerId);
            }

            _trackerRepository.DeleteTracker(trackerId);
        }
    }
}