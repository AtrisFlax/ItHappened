using AutoMapper;
using ItHappened.Domain.Statistics;
using NUnit.Framework;
using ItHappened.Infrastructure.Mappers;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.StatisticsCalculatorsTestingConstants;

namespace ItHappened.UnitTests
{
    public class DomainToRequest
    {
        [Test]
        //some example of mapping from ISingleTrackerFact to Concrete Dto objects for EF
        public void FactToConcreteTypeFact()
        {
            //arrange
            var facts = new ISingleTrackerFact[]
            {
                new AverageScaleTrackerFact("Scale", "description1", 1.0, 1.1, "unit1"),
                new AverageRatingTrackerFact("Rating", "description2", 2.1, 2.2)
            };

            var mapperCfg = new MapperConfiguration(
                cfg =>
                {
                    //map TrackerRequest => TrackerCustomizationSettings
                    cfg.CreateMap<AverageScaleTrackerFact, AverageScaleTrackerFactDto>();
                    cfg.CreateMap<AverageRatingTrackerFact, AverageRatingTrackerFactDto>();
                });
            var mapper = new Mapper(mapperCfg);
            AverageScaleTrackerFactDto averageScaleTrackerFactDto = null;
            AverageRatingTrackerFactDto averageRatingTrackerFactDto = null;

            //act
            foreach (var fact in facts)
            {
                switch (fact)
                {
                    case AverageScaleTrackerFact concreteFact:
                    {
                        averageScaleTrackerFactDto = mapper.Map<AverageScaleTrackerFactDto>(concreteFact);
                        break;
                    }
                    case AverageRatingTrackerFact concreteFact:
                    {
                        averageRatingTrackerFactDto = mapper.Map<AverageRatingTrackerFactDto>(concreteFact);
                        break;
                    }
                }
            }
            
            //assert
            Assert.AreEqual("Scale", averageScaleTrackerFactDto?.FactName);
            Assert.AreEqual("description1", averageScaleTrackerFactDto?.Description);
            Assert.AreEqual(1.0, averageScaleTrackerFactDto?.Priority);
            Assert.AreEqual(1.1000000000000001, averageScaleTrackerFactDto?.AverageValue, AverageAccuracy);
            Assert.AreEqual("unit1", averageScaleTrackerFactDto?.MeasurementUnit);
            
            Assert.AreEqual("Rating", averageRatingTrackerFactDto?.FactName);
            Assert.AreEqual("description2", averageRatingTrackerFactDto?.Description);
            Assert.AreEqual(2.1, averageRatingTrackerFactDto?.Priority);
            Assert.AreEqual(2.2, averageRatingTrackerFactDto?.AverageRating);
        }
    }
}