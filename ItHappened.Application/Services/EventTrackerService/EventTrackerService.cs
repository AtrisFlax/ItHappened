using System;
using System.Collections.Generic;
using ItHappened.Domain;
using LanguageExt;
using Serilog;

namespace ItHappened.Application.Services.EventTrackerService
{
    public class EventTrackerService : IEventTrackerService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventTrackerRepository _eventTrackerRepository;

        public EventTrackerService(IEventTrackerRepository eventTrackerRepository, IEventRepository eventRepository)
        {
            _eventTrackerRepository = eventTrackerRepository;
            _eventRepository = eventRepository;
        }

        public bool DeleteTracker(Guid trackerCreatorId, Guid trackerId)
        {
            var tracker = _eventTrackerRepository.LoadEventFromTracker(trackerId);
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

        public Guid CreateTracker(
            Guid creatorId,
            string trackerName,
            bool hasPhoto = false,
            bool hasScale = false,
            string scaleMeasurementUnit = "",
            bool hasRating = false,
            bool hashGeoTag = false,
            bool hasComment = false)
        {
            var trackerBuilder = EventTrackerBuilder
                .Tracker(creatorId, Guid.NewGuid(), trackerName);
            if (hasPhoto)
            {
                trackerBuilder = trackerBuilder.WithPhoto();
            }

            if (hasScale)
            {
                trackerBuilder = trackerBuilder.WithScale(scaleMeasurementUnit);
            }

            if (hasRating)
            {
                trackerBuilder = trackerBuilder.WithRating();
            }

            if (hashGeoTag)
            {
                trackerBuilder = trackerBuilder.WithGeoTag();
            }

            if (hasComment)
            {
                trackerBuilder = trackerBuilder.WithComment();
            }

            var tracker = trackerBuilder.Build();
            _eventTrackerRepository.SaveEventInTracker(tracker);
            return tracker.TrackerId;
        }

        public IEnumerable<EventTracker> GetAllTrackers(Guid trackerCreatorId)
        {
            return _eventTrackerRepository.LoadUserTrackers(trackerCreatorId);
        }

        public Option<EventTracker> GetTracker(Guid trackerCreatorId, Guid trackerId)
        {
            var tracker = _eventTrackerRepository.LoadEventFromTracker(trackerId);
            if (trackerCreatorId == tracker.CreatorId)
            {
                Log.Information(
                    $"Can't return tracker with trackerId={trackerId} trackerCreatorId={trackerCreatorId}. TrackerCreatorId does not match");
                return Option<EventTracker>.Some(tracker);
            }

            Log.Information($"Getting tracker with trackerId={trackerId} trackerCreatorId={trackerCreatorId}");
            return Option<EventTracker>.None;
        }

        public bool AddEventToTracker(Guid trackerCreatorId, Guid trackerId, Event @event)
        {
            var tracker = _eventTrackerRepository.LoadEventFromTracker(trackerId);
            tracker.AddEvent(@event);
            if (trackerCreatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't add event to tracker trackerId={trackerId} trackerCreatorId={trackerCreatorId}. TrackerCreatorId does not match");
                return false;
            }
            _eventTrackerRepository.SaveEventInTracker(tracker);
            _eventRepository.AddEvent(@event);
            Log.Information(
                $"Event Added trackerId={trackerId} trackerCreatorId={trackerCreatorId} eventId={@event.Id}");
            return true;
        }

        public bool RemoveEventFromTracker(Guid trackerCreatorId, Guid trackerId, Guid eventId)
        {
            var tracker = _eventTrackerRepository.LoadEventFromTracker(trackerId);
            if (trackerCreatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't remove tracker with trackerId={trackerId} eventId={eventId} trackerCreatorId={trackerCreatorId}. TrackerCreatorId does not match");
                return false;
            }

            var eventToRemove = _eventRepository.LoadEvent(eventId);
            tracker.RemoveEvent(eventToRemove);
            _eventRepository.DeleteEvent(eventId);
            _eventTrackerRepository.SaveEventInTracker(tracker);
            Log.Information(
                $"Event from tracker has deleted.  trackerId={trackerId}  eventId={eventId} trackerCreatorId={trackerCreatorId}");
            return true;
        }

        public bool EditEventInTracker(Guid trackerCreatorId, Guid trackerId, Guid eventId, Event newEvent)
        {
            var tracker = _eventTrackerRepository.LoadEventFromTracker(trackerId);
            if (trackerCreatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't edit event in tracker trackerId={trackerId} trackerCreatorId={trackerCreatorId}. TrackerCreatorId does not match");
                return false;
            }
            var oldEvent = _eventRepository.LoadEvent(eventId);
            if (oldEvent == null)
            {
                Log.Information(
                    $"Can't edit event in tracker trackerId={trackerId} trackerCreatorId={trackerCreatorId} eventId={eventId}. Event does not existed");
                return false;
            } 
            if (eventId != newEvent.Id)
            {
                Log.Information(
                    $"Can't edit event in tracker trackerId={trackerId} trackerCreatorId={trackerCreatorId} eventId={eventId}. EventId does not match");
                return false;
            }
            _eventRepository.DeleteEvent(eventId);
            _eventRepository.AddEvent(newEvent);
            _eventTrackerRepository.SaveEventInTracker(tracker);
            Log.Information(
                $"Event Added trackerId={trackerId} trackerCreatorId={trackerCreatorId} eventId={newEvent.Id}");
            return true;
        }

        public Option<IList<Event>> GetAllEventsFromTracker(Guid trackerId, Guid trackerCreatorId)
        {
            var tracker = _eventTrackerRepository.LoadEventFromTracker(trackerId);
            if (trackerCreatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't get events from tracker trackerId={trackerId} trackerCreatorId={trackerCreatorId}. TrackerCreatorId does not match");
                return Option<IList<Event>>.None;
            }

            Log.Information($"Returned events from trackerId={trackerId}");
            return Option<IList<Event>>.Some(tracker.Events);
        }

        public Option<IReadOnlyCollection<Event>> GetEventsFiltratedByTime(Guid trackerCreatorId,
            Guid trackerId,
            DateTimeOffset from, DateTimeOffset to)
        {
            var tracker = _eventTrackerRepository.LoadEventFromTracker(trackerId);
            if (trackerCreatorId != tracker.CreatorId)
            {
                Log.Information(
                    $"Can't filter events from tracker trackerId={trackerId} trackerCreatorId={trackerCreatorId}. TrackerCreatorId does not match");
                return Option<IReadOnlyCollection<Event>>.None;
            }
            var requiredTracker = _eventTrackerRepository.LoadEventFromTracker(trackerId);
            Log.Information($"Get Filtered from {from} to {to} events from trackerId={trackerId}");
            return Option<IReadOnlyCollection<Event>>.Some(requiredTracker.FilterEventsByTimeSpan(from, to));
        }
    }
}