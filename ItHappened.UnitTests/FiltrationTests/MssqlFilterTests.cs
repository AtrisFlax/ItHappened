using System;
using System.Linq;
using AutoMapper;
using ItHappened.Api.Mapping;
using ItHappened.Api.Models.Requests;
using ItHappened.Domain;
using ItHappened.Infrastructure;
using NUnit.Framework;

namespace ItHappened.UnitTests.FiltrationTests
{
    public class MssqlFilterTests
    {
        [Test]
        public void DateFilterUpperAndLowerBound()
        {
            //arrange
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new RequestToDomainProfile()); });
            var mapper = new Mapper(mapperConfig);
            var eventFilterRequest = new EventFilterRequest
            {
                FromDateTime = new DateTime(2019, 10, 14, 18, 01, 00),
                ToDateTime = new DateTime(2020, 10, 14, 18, 00, 00)
            };
            var filterStringPredicates = mapper.Map<EventFilter>(eventFilterRequest);

            //act
            var filter = new MssqlFilter().CreateFilterMsSqlPredicates(filterStringPredicates).ToList();

            //
            Assert.AreEqual(filter[0], "HappensDate >= '2019-10-14T18:01:00'");
            Assert.AreEqual(filter[1], "HappensDate <= '2020-10-14T18:00:00'");
        }

        [Test]
        public void DateFilterUpperBound()
        {
            //arrange
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new RequestToDomainProfile()); });
            var mapper = new Mapper(mapperConfig);
            var eventFilterRequest = new EventFilterRequest
            {
                ToDateTime = new DateTime(2020, 10, 14, 18, 00, 00)
            };
            var filtersData = mapper.Map<EventFilter>(eventFilterRequest);

            //act
            var filterStringPredicates = new MssqlFilter().CreateFilterMsSqlPredicates(filtersData).ToList();

            //
            Assert.AreEqual(filterStringPredicates[0], "HappensDate <= '2020-10-14T18:00:00'");
        }

        [Test]
        public void DateFilterLowerBound()
        {
            //arrange
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new RequestToDomainProfile()); });
            var mapper = new Mapper(mapperConfig);
            var eventFilterRequest = new EventFilterRequest
            {
                FromDateTime = new DateTime(2019, 10, 14, 18, 01, 00),
            };
            var filterStringPredicates = mapper.Map<EventFilter>(eventFilterRequest);

            //act
            var filter = new MssqlFilter().CreateFilterMsSqlPredicates(filterStringPredicates).ToList();

            //
            Assert.AreEqual(filter[0], "HappensDate >= '2019-10-14T18:01:00'");
        }

        [Test]
        public void RatingFilterUpperAndLowerBound()
        {
            //arrange
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new RequestToDomainProfile()); });
            var mapper = new Mapper(mapperConfig);
            var eventFilterRequest = new EventFilterRequest
            {
                LowerLimitRating = 2,
                UpperLimitRating = 5.5
            };
            var filtersData = mapper.Map<EventFilter>(eventFilterRequest);

            //act
            var filterStringPredicates = new MssqlFilter().CreateFilterMsSqlPredicates(filtersData).ToList();

            //assert
            Assert.AreEqual(filterStringPredicates[0], "rating >= 2");
            Assert.AreEqual(filterStringPredicates[1], "rating <= 5.5");
        }

        [Test]
        public void RatingFilterLowerBound()
        {
            //arrange
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new RequestToDomainProfile()); });
            var mapper = new Mapper(mapperConfig);
            var eventFilterRequest = new EventFilterRequest
            {
                LowerLimitRating = 2,
            };
            var filtersData = mapper.Map<EventFilter>(eventFilterRequest);

            //act
            var filterStringPredicates = new MssqlFilter().CreateFilterMsSqlPredicates(filtersData).ToList();

            //assert
            Assert.AreEqual(filterStringPredicates[0], "rating >= 2");
        }

        [Test]
        public void RatingFilterUpperBound()
        {
            //arrange
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new RequestToDomainProfile()); });
            var mapper = new Mapper(mapperConfig);
            var eventFilterRequest = new EventFilterRequest
            {
                UpperLimitRating = 5.5
            };
            var filtersData = mapper.Map<EventFilter>(eventFilterRequest);

            //act
            var filterStringPredicates = new MssqlFilter().CreateFilterMsSqlPredicates(filtersData).ToList();

            //assert
            Assert.AreEqual(filterStringPredicates[0], "rating <= 5.5");
        }

        [Test]
        public void ScaleFilterUpperAndLowerBound()
        {
            //arrange
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new RequestToDomainProfile()); });
            var mapper = new Mapper(mapperConfig);
            var eventFilterRequest = new EventFilterRequest
            {
                ScaleLowerLimit = 2,
                ScaleUpperLimit = 5.5
            };
            var filtersData = mapper.Map<EventFilter>(eventFilterRequest);

            //act
            var filterStringPredicates = new MssqlFilter().CreateFilterMsSqlPredicates(filtersData).ToList();

            //assert
            Assert.AreEqual(filterStringPredicates[0], "scale >= 2");
            Assert.AreEqual(filterStringPredicates[1], "scale <= 5.5");
        }

        [Test]
        public void ScaleFilterLowerBound()
        {
            //arrange
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new RequestToDomainProfile()); });
            var mapper = new Mapper(mapperConfig);
            var eventFilterRequest = new EventFilterRequest
            {
                ScaleLowerLimit = 2,
            };
            var filtersData = mapper.Map<EventFilter>(eventFilterRequest);

            //act
            var filterStringPredicates = new MssqlFilter().CreateFilterMsSqlPredicates(filtersData).ToList();

            //assert
            Assert.AreEqual(filterStringPredicates[0], "scale >= 2");
        }

        [Test]
        public void ScaleFilterUpperBound()
        {
            //arrange
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new RequestToDomainProfile()); });
            var mapper = new Mapper(mapperConfig);
            var eventFilterRequest = new EventFilterRequest
            {
                ScaleUpperLimit = 5.5
            };
            var filtersData = mapper.Map<EventFilter>(eventFilterRequest);

            //act
            var filterStringPredicates = new MssqlFilter().CreateFilterMsSqlPredicates(filtersData).ToList();

            //assert
            Assert.AreEqual(filterStringPredicates[0], "scale <= 5.5");
        }

        [Test]
        public void GetTagBounds()
        {
            //arrange
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new RequestToDomainProfile()); });
            var mapper = new Mapper(mapperConfig);
            var eventFilterRequest = new EventFilterRequest
            {
                GpsLatLeftCorner = 10.0,
                GpsLngLeftCorner = 11.0,
                GpsLatRightCorner = 20.0,
                GpsLngRightCorner = 21.0
            };
            var filtersData = mapper.Map<EventFilter>(eventFilterRequest);

            //act
            var filterStringPredicates = new MssqlFilter().CreateFilterMsSqlPredicates(filtersData).ToList();

            //assert
            Assert.AreEqual(filterStringPredicates[0], "latitudeGeo >= 10");
            Assert.AreEqual(filterStringPredicates[1], "latitudeGeo <= 20");
            Assert.AreEqual(filterStringPredicates[2], "longitudeGeo >= 11");
            Assert.AreEqual(filterStringPredicates[3], "longitudeGeo <= 21");
        }
        
        
        [Test]
        public void Comment()
        {
            //arrange
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new RequestToDomainProfile()); });
            var mapper = new Mapper(mapperConfig);
            var eventFilterRequest = new EventFilterRequest
            {
                SubstringForMatching = "SomeComment",
            };
            var filtersData = mapper.Map<EventFilter>(eventFilterRequest);

            //act
            var filterStringPredicates = new MssqlFilter().CreateFilterMsSqlPredicates(filtersData).ToList();

            //assert
            Assert.AreEqual(filterStringPredicates[0], "comment LIKE 'SomeComment'");
        }
        
        [Test]
        public void AllPredicates()
        {
            //arrange
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new RequestToDomainProfile()); });
            var mapper = new Mapper(mapperConfig);
            var eventFilterRequest = new EventFilterRequest
            {
                FromDateTime = new DateTime(2020, 10, 14, 18, 01, 00),
                ToDateTime = new DateTime(2019, 10, 14, 18, 00, 00),
                LowerLimitRating = 0.2,
                UpperLimitRating = 0.5,
                SubstringForMatching = "aaa",
                ScaleLowerLimit = 0.3,
                ScaleUpperLimit = 400,
                GpsLatLeftCorner = 15.8,
                GpsLngLeftCorner = 17.9,
                GpsLatRightCorner = 32.4,
                GpsLngRightCorner = 78.4
            };
            var filtersData = mapper.Map<EventFilter>(eventFilterRequest);

            //act
            var filterStringPredicates = new MssqlFilter().CreateFilterMsSqlPredicates(filtersData).ToList();

            //assert
            Assert.AreEqual(filterStringPredicates[0], "HappensDate >= '2020-10-14T18:01:00'");
            Assert.AreEqual(filterStringPredicates[1], "HappensDate <= '2019-10-14T18:00:00'");
            Assert.AreEqual(filterStringPredicates[2], "scale >= 0.3");
            Assert.AreEqual(filterStringPredicates[3], "scale <= 400");
            Assert.AreEqual(filterStringPredicates[4], "rating >= 0.2");
            Assert.AreEqual(filterStringPredicates[5], "rating <= 0.5");
            Assert.AreEqual(filterStringPredicates[6], "comment LIKE 'aaa'");
            Assert.AreEqual(filterStringPredicates[7], "latitudeGeo >= 15.8");
            Assert.AreEqual(filterStringPredicates[8], "latitudeGeo <= 32.4");
            Assert.AreEqual(filterStringPredicates[9], "longitudeGeo >= 17.9");
            Assert.AreEqual(filterStringPredicates[10], "longitudeGeo <= 78.4");
        }
    }
}