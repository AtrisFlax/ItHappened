using System;
using System.Collections.Generic;
using ItHappened.Domain;
using LanguageExt;
using Serilog;

namespace ItHappened.Application.Services.EventTrackerService
{
    public class EventTrackerService : IEventTrackerService
    {
        private readonly IEventTrackerRepository _eventTrackerRepository;
        private readonly IEventRepository _eventRepository;

        public EventTrackerService(IEventTrackerRepository eventTrackerRepository, IEventRepository eventRepository)
        {
            _eventTrackerRepository = eventTrackerRepository;
            _eventRepository = eventRepository;
        }

        public Guid CreateTracker(
            Guid creatorId,
            string trackerName,
            bool hasPhoto = false,
            bool hasScale = false,
            bool hasRating = false,
            bool hashGeoTag = false,
            bool hasComment = false)
        {
            var eventTracker = new EventTracker(Guid.NewGuid(), trackerName, new List<Event>(), creatorId);
            return eventTracker.Id;
        }

        public bool DeleteTracker(Guid trackerId, Guid trackerCreatorId)
        {
            var tracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            if (trackerCreatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't remove tracker with trackerId={trackerId} trackerCreatorId={trackerCreatorId}. TrackerCreatorId does not match");
                return false;
            }

            _eventTrackerRepository.DeleteEventTracker(trackerId);
            Log.Information($"Tracker deleted with trackerId={trackerId} trackerCreatorId={trackerCreatorId}");
            return true;
        }

        public IEnumerable<EventTracker> GetAllTrackers(Guid userId)
        {
            return _eventTrackerRepository.LoadUserTrackers(userId);
        }

        public bool AddEventToTracker(Guid trackerId, Guid trackerCreatorId, Event @event)
        {
            var tracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            if (trackerCreatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't add event to tracker trackerId={trackerId} trackerCreatorId={trackerCreatorId}. TrackerCreatorId does not match");
                return false;
            }

            _eventRepository.AddEvent(@event);
            Log.Information(
                $"Event Added trackerId={trackerId} trackerCreatorId={trackerCreatorId} eventId={@event.Id}");
            return true;
        }

        public bool RemoveEventFromTracker(Guid trackerId, Guid eventId, Guid trackerCreatorId)
        {
            var tracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            if (trackerCreatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't remove tracker with trackerId={trackerId} eventId={eventId} trackerCreatorId={trackerCreatorId}. TrackerCreatorId does not match");
                return false;
            }

            var eventToRemove = _eventRepository.TryLoadEvent(eventId);
            tracker.RemoveEvent(eventToRemove);
            Log.Information(
                $"Event from tracker has deleted.  trackerId={trackerId}  eventId={eventId} trackerCreatorId={trackerCreatorId}");
            return true;
        }

        public bool EditEventTracker(Guid trackerId, Guid trackerCreatorId, Event @event)
        {
            var tracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            if (trackerCreatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't add event to tracker trackerId={trackerId} trackerCreatorId={trackerCreatorId}. TrackerCreatorId does not match");
                return false;
            }

            _eventRepository.AddEvent(@event);
            Log.Information(
                $"Event Added trackerId={trackerId} trackerCreatorId={trackerCreatorId} eventId={@event.Id}");
            return true;
        }

        public IEnumerable<EventTracker> GetAllEventsFromTracker(Guid tracker, Guid trackerCreatorId)
        {
            return Array.Empty<EventTracker>();
        }

        public Option<IReadOnlyCollection<Event>> FilterByTime(Guid trackerId, Guid trackerCreatorId,
            DateTimeOffset from, DateTimeOffset to)
        {
            var requiredTracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            return trackerCreatorId != requiredTracker.CreatorId
                ? Option<IReadOnlyCollection<Event>>.None
                : Option<IReadOnlyCollection<Event>>.Some(requiredTracker.FilterEventsByTimeSpan(from, to));
        }
    }
}