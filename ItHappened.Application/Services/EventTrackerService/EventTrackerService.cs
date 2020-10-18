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

        public bool DeleteTracker(Guid trackerCreatorId, Guid trackerId)
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

        public IEnumerable<EventTracker> GetAllTrackers(Guid trackerCreatorId)
        {
            return _eventTrackerRepository.LoadUserTrackers(trackerCreatorId);
        }

        public Option<EventTracker> GetTracker(Guid trackerCreatorId, Guid trackerId)
        {
            var tracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            if (trackerCreatorId == tracker.CreatorId)
            {
                Log.Information($"Can't return tracker with trackerId={trackerId} trackerCreatorId={trackerCreatorId}. TrackerCreatorId does not match");
                return Option<EventTracker>.Some(tracker);
            }
            Log.Information($"Getting tracker with trackerId={trackerId} trackerCreatorId={trackerCreatorId}");
            return Option<EventTracker>.None;
        }

        public bool AddEventToTracker(Guid trackerCreatorId, Guid trackerId, Event @event)
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

        public bool RemoveEventFromTracker(Guid trackerCreatorId, Guid trackerId, Guid eventId)
        {
            var tracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            if (trackerCreatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't remove tracker with trackerId={trackerId} eventId={eventId} trackerCreatorId={trackerCreatorId}. TrackerCreatorId does not match");
                return false;
            }
            var eventToRemove = _eventRepository.LoadEvent(eventId);
            tracker.RemoveEvent(eventToRemove);
            Log.Information(
                $"Event from tracker has deleted.  trackerId={trackerId}  eventId={eventId} trackerCreatorId={trackerCreatorId}");
            return true;
        }

        public bool EditEventInTracker(Guid trackerCreatorId, Guid trackerId, Guid eventId, Event @event)
        {
            var tracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            if (trackerCreatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't add event to tracker trackerId={trackerId} trackerCreatorId={trackerCreatorId}. TrackerCreatorId does not match");
                return false;
            }
            _eventRepository.DeleteEvent(eventId);
            _eventRepository.AddEvent(@event);
            Log.Information(
                $"Event Added trackerId={trackerId} trackerCreatorId={trackerCreatorId} eventId={@event.Id}");
            return true;
        }

        public Option<IList<Event>> GetAllEventsFromTracker(Guid trackerId, Guid trackerCreatorId)
        {
            var tracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            if (trackerCreatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't get events from tracker trackerId={trackerId} trackerCreatorId={trackerCreatorId}. TrackerCreatorId does not match");
                return Option<IList<Event>>.None;
            }
            Log.Information($"Returned events from trackerId={trackerId}");
            return Option<IList<Event>>.Some(tracker.Events);
        }

        public Option<IReadOnlyCollection<Event>> FilterByTime(Guid trackerCreatorId,
            Guid trackerId,
            DateTimeOffset from, DateTimeOffset to)
        {
            var tracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            if (trackerCreatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't filter events from tracker trackerId={trackerId} trackerCreatorId={trackerCreatorId}. TrackerCreatorId does not match");
                return Option<IReadOnlyCollection<Event>>.None;
            }
            var requiredTracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            Log.Information($"Get Filtered from {from} to {to} events from trackerId={trackerId}");
            return Option<IReadOnlyCollection<Event>>.Some(requiredTracker.FilterEventsByTimeSpan(from, to));
        }
    }
}