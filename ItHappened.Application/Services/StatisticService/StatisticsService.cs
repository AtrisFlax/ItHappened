using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IGeneralFactsRepository _generalFactsRepository;
        private readonly ISpecificFactsRepository _specificFactsRepository;
        private readonly IManualStatisticGenerator _manualStatisticGenerator;
        public StatisticsService(IGeneralFactsRepository generalFactsRepository, 
            ISpecificFactsRepository specificFactsRepository, IManualStatisticGenerator manualStatisticGenerator)
        {
            _generalFactsRepository = generalFactsRepository;
            _specificFactsRepository = specificFactsRepository;
            _manualStatisticGenerator = manualStatisticGenerator;
        }

        public void UpdateTrackerSpecificFacts(Guid trackerId)
        {
            _manualStatisticGenerator.UpdateTrackerSpecificFacts(trackerId);
        }
        
        public void UpdateUserGeneralFacts(Guid userId)
        {
            _manualStatisticGenerator.UpdateUserGeneralFacts(userId);
        }
        public IReadOnlyCollection<IGeneralFact> GetGeneralTrackersFacts(Guid userId)
        {
            //проверки на наличие фактов в репозитории, на то, свои ли факты запрашивает initiator
            var statisticFacts = _generalFactsRepository.LoadUserGeneralFacts(userId);
            return statisticFacts;
        }

        public IReadOnlyCollection<ISpecificFact> GetSpecificTrackerFacts(Guid userId, Guid trackerId)
        {
            //проверки на наличие фактов в репозитории, на то, свои ли факты запрашивает initiator, принадлежит ли трекер инциатору 
            var statisticFacts = _specificFactsRepository.LoadTrackerSpecificFacts(trackerId);
            return statisticFacts;
        }
    }
}