using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;

namespace ItHappened.Application.Services.TrackerService
{
    public class TrackerService : ITrackerService
    {
        private readonly ITrackerRepository _trackerRepository;
        private readonly IEventRepository _eventRepository;
        public TrackerService(ITrackerRepository trackerRepository, IEventRepository eventRepository)
        {
            _trackerRepository = trackerRepository;
            _eventRepository = eventRepository;
        }
        
        public EventTracker CreateEventTracker(Guid creatorId, string name, TrackerCustomizationSettings customizationSettings)
        {
            var id = Guid.NewGuid();
            var tracker = new EventTracker(id, creatorId, name, customizationSettings);
            _trackerRepository.SaveTracker(tracker);
            return tracker;
        }

        public EventTracker GetEventTracker(Guid actorId, Guid trackerId)
        {
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (actorId != tracker.CreatorId)
                throw new Exception();
            return tracker;
        }
        
        public IReadOnlyCollection<EventTracker> GetEventTrackers(Guid actorId)
        {
            var trackers = _trackerRepository.LoadAllUserTrackers(actorId);
            return trackers.ToList();
        }
        
        public EventTracker EditEventTracker(Guid actorId,
            Guid trackerId,
            string name,
            TrackerCustomizationSettings customizationSettings)
        {
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (actorId != tracker.CreatorId)
                throw new Exception();
            
            var updatedTracker = new EventTracker(tracker.Id, tracker.CreatorId, name, customizationSettings);
            _trackerRepository.UpdateTracker(updatedTracker);
            return updatedTracker;
        }

        public EventTracker DeleteEventTracker(Guid actorId, Guid trackerId)
        {
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (actorId != tracker.CreatorId)
                throw new Exception();

            _trackerRepository.DeleteTracker(trackerId);
            return tracker;
        }
    }
}