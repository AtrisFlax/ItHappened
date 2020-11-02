using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Dto;
using ItHappened.Infrastructure.Mappers;

//TODO !!!тут все очень плохо!!!  читать дальше про EF и Automapper 
namespace ItHappened.Infrastructure.EFCoreRepositories
{
    // ReSharper disable once InconsistentNaming
    public class EFFactsRepository
    {
        private readonly ItHappenedDbContext _context;
        private readonly IMapper _mapper;

        public EFFactsRepository(ItHappenedDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void CreateTrackerSpecificFacts(Guid userId, Guid trackerId,
            IReadOnlyCollection<ISingleTrackerFact> facts)
        {
            foreach (var fact in facts)
            {
                switch (fact)
                {
                    case AverageRatingTrackerFact concreteFact:
                    {
                        var dto = _mapper.Map<AverageRatingTrackerFactDto>(concreteFact);
                        dto.UserId = userId;
                        dto.TrackerId = trackerId;
                        _context.AverageRatingTrackerFactsDto.Add(dto);
                        break;
                    }

                    case AverageScaleTrackerFact concreteFact:
                    {
                        var dto = _mapper.Map<AverageScaleTrackerFactDto>(concreteFact);
                        dto.UserId = userId;
                        dto.TrackerId = trackerId;
                        _context.AverageScaleTrackerFactsDto.Add(dto);
                        break;
                    }
                    case BestRatingEventFact concreteFact:
                    {
                        var dto = _mapper.Map<BestRatingEventFactDto>(concreteFact);
                        dto.UserId = userId;
                        dto.TrackerId = trackerId;
                        _context.BestRatingEventFactsDto.Add(dto);
                        break;
                    }

                    case LongestBreakTrackerFact concreteFact:
                    {
                        var dto = _mapper.Map<LongestBreakTrackerFactDto>(concreteFact);
                        dto.UserId = userId;
                        dto.TrackerId = trackerId;
                        _context.LongestBreakTrackerFactsDto.Add(dto);
                        break;
                    }

                    case OccursOnCertainDaysOfTheWeekTrackerFact concreteFact:
                    {
                        var dto = _mapper.Map<OccursOnCertainDaysOfTheWeekTrackerFactDto>(concreteFact);
                        dto.UserId = userId;
                        dto.TrackerId = trackerId;
                        _context.OccursOnCertainDaysOfTheWeekTrackerFactsDto.Add(dto);
                        break;
                    }
                    case SingleTrackerEventsCountFact concreteFact:
                    {
                        var dto = _mapper.Map<SingleTrackerEventsCountFactDto>(concreteFact);
                        dto.UserId = userId;
                        dto.TrackerId = trackerId;
                        _context.SingleTrackerEventsCountFactsDto.Add(dto);
                        break;
                    }
                    case SpecificDayTimeFact concreteFact:
                    {
                        var dto = _mapper.Map<SpecificDayTimeFactDto>(concreteFact);
                        dto.UserId = userId;
                        dto.TrackerId = trackerId;
                        _context.SpecificDayTimeFactsDto.Add(dto);
                        break;
                    }
                    case SumScaleTrackerFact concreteFact:
                    {
                        var dto = _mapper.Map<SumScaleTrackerFactDto>(concreteFact);
                        dto.UserId = userId;
                        dto.TrackerId = trackerId;
                        _context.SumScaleTrackerFactsDto.Add(dto);
                        break;
                    }
                    case WorstRatingEventFact concreteFact:
                    {
                        var dto = _mapper.Map<WorstRatingEventFactDto>(concreteFact);
                        dto.UserId = userId;
                        dto.TrackerId = trackerId;
                        _context.WorstRatingEventFactsDto.Add(dto);
                        break;
                    }
                }
            }
        }

        public void CreateTrackerSpecificFacts(Guid userId, IReadOnlyCollection<IMultipleTrackersFact> facts)
        {
            foreach (var fact in facts)
            {
                switch (fact)
                {
                    case EventsCountTrackersFact concreteFact:
                    {
                        var dto = _mapper.Map<EventsCountTrackersFactDto>(concreteFact);
                        dto.UserId = userId;
                        dto.TrackerId = null; //TrackerId == null for general fact;
                        _context.EventsCountTrackersFactsDto.Add(dto);
                        break;
                    }
                    case MostEventfulDayTrackersFact concreteFact:
                    {
                        var dto = _mapper.Map<MostEventfulDayTrackersFactDto>(concreteFact);
                        dto.UserId = userId;
                        dto.TrackerId = null; //TrackerId == null for general fact;
                        _context.MostEventfulDayTrackersFactsDto.Add(dto);
                        break;
                    }

                    case MostEventfulWeekTrackersFact concreteFact:
                    {
                        var dto = _mapper.Map<MostEventfulWeekTrackersFactDto>(concreteFact);
                        dto.UserId = userId;
                        dto.TrackerId = null; //TrackerId == null for general fact;
                        _context.MostEventfulWeekTrackersFactsDto.Add(dto);
                        break;
                    }

                    case MostFrequentEventTrackersFact concreteFact:
                    {
                        var dto = _mapper.Map<MostFrequentEventTrackersFactDto>(concreteFact);
                        dto.UserId = userId;
                        dto.TrackerId = null; //TrackerId == null for general fact;
                        _context.MostFrequentEventTrackersFactsDto.Add(dto);
                        break;
                    }
                }
            }
        }


        public IReadOnlyCollection<IMultipleTrackersFact> LoadUserGeneralFacts(Guid userId)
        {
            var eventsCountTrackersFactDto =
                _context.EventsCountTrackersFactsDto
                    .Filter(fact => fact.UserId == userId && fact.TrackerId == null);
            var mostEventfulDayTrackersFactDto =
                _context.MostEventfulDayTrackersFactsDto
                    .Filter(fact => fact.UserId == userId && fact.TrackerId == null);
            var mostEventfulWeekTrackersFactDto =
                _context.MostEventfulWeekTrackersFactsDto
                    .Filter(fact => fact.UserId == userId && fact.TrackerId == null);
            var mostFrequentEventTrackersFactDto =
                _context.MostFrequentEventTrackersFactsDto
                    .Filter(fact => fact.UserId == userId && fact.TrackerId == null);

            var eventsCountTrackersFact = _mapper.Map<EventsCountTrackersFact>(eventsCountTrackersFactDto);
            var mostEventfulDayTrackersFact = _mapper.Map<MostEventfulDayTrackersFact>(mostEventfulDayTrackersFactDto);
            var mostEventfulWeekTrackersFact = _mapper.Map<MostEventfulWeekTrackersFact>(mostEventfulWeekTrackersFactDto);
            var mostFrequentEventTrackersFact = _mapper.Map<MostFrequentEventTrackersFact>(mostFrequentEventTrackersFactDto);

            return new List<IMultipleTrackersFact>
            {
                eventsCountTrackersFact,
                mostEventfulDayTrackersFact,
                mostEventfulWeekTrackersFact,
                mostFrequentEventTrackersFact
            };
        }

        public IReadOnlyCollection<ISingleTrackerFact> ReadTrackerSpecificFacts(Guid userId, Guid trackerId)
        {
            var averageRatingTrackerFactDto = _context.AverageRatingTrackerFactsDto
                .Filter(fact => fact.UserId == userId && fact.TrackerId == trackerId);
            var averageScaleTrackerFactDto = _context.AverageScaleTrackerFactsDto
                .Filter(fact => fact.UserId == userId && fact.TrackerId == trackerId);
            var bestRatingEventFactDto = _context.BestRatingEventFactsDto
                .Filter(fact => fact.UserId == userId && fact.TrackerId == trackerId);
            var longestBreakTrackerFactDto = _context.LongestBreakTrackerFactsDto
                .Filter(fact => fact.UserId == userId && fact.TrackerId == trackerId);
            var occursOnCertainDaysOfTheWeekTrackerFactDto = _context.OccursOnCertainDaysOfTheWeekTrackerFactsDto
                .Filter(fact => fact.UserId == userId && fact.TrackerId == trackerId);
            var singleTrackerEventsCountFactDto = _context.SingleTrackerEventsCountFactsDto
                .Filter(fact => fact.UserId == userId && fact.TrackerId == trackerId);
            var specificDayTimeFactDto = _context.SpecificDayTimeFactsDto
                .Filter(fact => fact.UserId == userId && fact.TrackerId == trackerId);
            var sumScaleTrackerFactDto = _context.SumScaleTrackerFactsDto
                .Filter(fact => fact.UserId == userId && fact.TrackerId == trackerId);
            var worstRatingEventFactDto = _context.WorstRatingEventFactsDto
                .Filter(fact => fact.UserId == userId && fact.TrackerId == trackerId);

            var averageRatingTrackerFact = _mapper.Map<AverageRatingTrackerFact>(averageRatingTrackerFactDto);
            var averageScaleTrackerFact = _mapper.Map<AverageScaleTrackerFact>(averageScaleTrackerFactDto);
            var bestRatingEventFact = _mapper.Map<BestRatingEventFact>(bestRatingEventFactDto);
            var longestBreakTrackerFact = _mapper.Map<LongestBreakTrackerFact>(longestBreakTrackerFactDto);
            var occursOnCertainDaysOfTheWeekTrackerFact = _mapper.Map<OccursOnCertainDaysOfTheWeekTrackerFact>(occursOnCertainDaysOfTheWeekTrackerFactDto);
            var singleTrackerEventsCountFact = _mapper.Map<SingleTrackerEventsCountFact>(singleTrackerEventsCountFactDto);
            var specificDayTimeFact = _mapper.Map<SpecificDayTimeFact>(specificDayTimeFactDto);
            var sumScaleTrackerFact = _mapper.Map<SumScaleTrackerFact>(sumScaleTrackerFactDto);
            var worstRatingEventFact = _mapper.Map<WorstRatingEventFact>(worstRatingEventFactDto);

            return new List<ISingleTrackerFact>
            {
                averageRatingTrackerFact,
                averageScaleTrackerFact,
                bestRatingEventFact,
                longestBreakTrackerFact,
                occursOnCertainDaysOfTheWeekTrackerFact,
                singleTrackerEventsCountFact,
                specificDayTimeFact,
                sumScaleTrackerFact,
                worstRatingEventFact
            };
        }

        public bool IsContainFactForTracker(Guid trackerId, Guid userId)
        {
            return _context.FactsDto.Any(fact => fact.UserId == userId && fact.TrackerId == trackerId);
        }

        public bool IsContainFactsForUser(Guid userId)
        {
            return _context.FactsDto.Any(fact => fact.UserId == userId && fact.TrackerId == null);
        }
    }
}