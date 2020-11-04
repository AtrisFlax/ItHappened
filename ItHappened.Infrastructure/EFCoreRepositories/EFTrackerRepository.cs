using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ItHappened.Application.Errors;
using ItHappened.Domain;
using ItHappened.Infrastructure.Mappers;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    // ReSharper disable once InconsistentNaming
    public class EFTrackerRepository : ITrackerRepository
    {
        private const int InsertKeyWithDuplicateRowErrorCode = 2601;
        private readonly ItHappenedDbContext _context;
        private readonly IMapper _mapper;

        public EFTrackerRepository(ItHappenedDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void SaveTracker(EventTracker newTracker)
        {
            var trackerDto = _mapper.Map<EventTrackerDto>(newTracker);
            _context.EventTrackers.Add(trackerDto);
        }

        public EventTracker LoadTracker(Guid eventTrackerId)
        {
            var trackerDto = _context.EventTrackers.Find(eventTrackerId);
            return _mapper.Map<EventTracker>(trackerDto);
        }

        public IReadOnlyCollection<EventTracker> LoadAllUserTrackers(Guid userId)
        {
            var trackersDto = _context.EventTrackers.Where(tracker => tracker.CreatorId == userId);
            return _mapper.Map<EventTracker[]>(trackersDto.ToList());
        }

        public void UpdateTracker(EventTracker eventTracker)
        {
            var oldTrackerDto = _context.EventTrackers.Find(eventTracker.Id);
            oldTrackerDto.IsUpdated = true;
            oldTrackerDto.Name = eventTracker.Name;
            oldTrackerDto.IsCommentRequired = eventTracker.CustomizationSettings.IsCommentRequired;
            oldTrackerDto.IsGeotagRequired = eventTracker.CustomizationSettings.IsGeotagRequired;
            oldTrackerDto.IsRatingRequired = eventTracker.CustomizationSettings.IsRatingRequired;
            oldTrackerDto.IsPhotoRequired = eventTracker.CustomizationSettings.IsPhotoRequired;
            oldTrackerDto.IsScaleRequired = eventTracker.CustomizationSettings.IsScaleRequired;
            oldTrackerDto.IsCustomizationRequired = eventTracker.CustomizationSettings.IsCustomizationRequired;
            oldTrackerDto.ScaleMeasurementUnit = eventTracker.CustomizationSettings.ScaleMeasurementUnit.ValueUnsafe();
        }

        public void DeleteTracker(Guid eventTrackerId)
        {
            //TODO test cascade delete 
            var trackerToDeleteDto = _context.EventTrackers.Find(eventTrackerId);
            _context.EventTrackers.Remove(trackerToDeleteDto);
            //TODO issue #180
            //Deleting without loading from the database
            // var toDelete = new EventDto {Id = eventTrackerId};
            // _context.Entry(toDelete).State = EntityState.Deleted;
            // _context.SaveChanges();
        }

        public bool IsContainTracker(Guid trackerId)
        {
            return _context.EventTrackers.Any(tracker => tracker.Id == trackerId);
        }

        public bool IsExistTrackerWithSameName(Guid creatorId, string trackerName)
        {
            return _context.EventTrackers.Any(tracker => tracker.CreatorId == creatorId && tracker.Name == trackerName);
        }
    }
}