using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using ItHappened.Domain;
using LanguageExt.Common;

namespace ItHappened.Application.Services.EventTrackerService
{
    public interface IEventTrackerService
    {
        EventTracker CreateEventTracker(Guid creatorId, string name, TrackerCustomizationSettings customizationSettings);
        EventTracker GetEventTracker(Guid actorId, Guid trackerId);
        IReadOnlyCollection<EventTracker> GetEventTrackers(Guid actorId);

        EventTracker EditEventTracker(Guid actorId,
            Guid trackerId,
            string name,
            TrackerCustomizationSettings customizationSettings);

        EventTracker DeleteEventTracker(Guid actorId, Guid trackerId);
    }

    public class EventTrackerService : IEventTrackerService
    {
        public EventTrackerService(IEventTrackerRepository eventTrackerRepository, IEventRepository eventRepository)
        {
            _eventTrackerRepository = eventTrackerRepository;
            _eventRepository = eventRepository;
        }
        
        public EventTracker CreateEventTracker(Guid creatorId, string name, TrackerCustomizationSettings customizationSettings)
        {
            var id = Guid.NewGuid();
            var tracker = new EventTracker(id, creatorId, name, customizationSettings);
            _eventTrackerRepository.SaveTracker(tracker);
            return tracker;
        }

        public EventTracker GetEventTracker(Guid actorId, Guid trackerId)
        {
            var tracker = _eventTrackerRepository.LoadTracker(trackerId);
            if (actorId != tracker.CreatorId)
                throw new Exception();
            return tracker;
        }
        
        public IReadOnlyCollection<EventTracker> GetEventTrackers(Guid actorId)
        {
            var trackers = _eventTrackerRepository.LoadAllUserTrackers(actorId);
            return trackers.ToList();
        }
        
        public EventTracker EditEventTracker(Guid actorId,
            Guid trackerId,
            string name,
            TrackerCustomizationSettings customizationSettings)
        {
            var tracker = _eventTrackerRepository.LoadTracker(trackerId);
            if (actorId != tracker.CreatorId)
                throw new Exception();
            
            var updatedTracker = new EventTracker(tracker.Id, tracker.CreatorId, name, customizationSettings);
            _eventTrackerRepository.UpdateTracker(updatedTracker);
            return updatedTracker;
        }

        public EventTracker DeleteEventTracker(Guid actorId, Guid trackerId)
        {
            var tracker = _eventTrackerRepository.LoadTracker(trackerId);
            if (actorId != tracker.CreatorId)
                throw new Exception();

            _eventTrackerRepository.DeleteTracker(trackerId);
            return tracker;
        }

        private readonly IEventTrackerRepository _eventTrackerRepository;
        private readonly IEventRepository _eventRepository;
    }
}