using System;
using System.Collections.Generic;
using ItHappened.Bll.Domain.Statistics.MultipleTrackersStatisticsFacts;
using ItHappened.Bll.Domain.Statistics.SingleTrackerStatisticsFacts;
using LanguageExt;

namespace ItHappened.Application.Services.StatisticService
{
    public interface IStatisticsService
    {
        public IReadOnlyCollection<Option<IMultipleTrackersStatisticsFact>> GetMultipleTrackersFacts(Guid userId);
        public IReadOnlyCollection<Option<ISingleTrackerStatisticsFact>> GetSingleTrackerFacts(Guid userId,
            Guid eventTrackerId);
    }
}