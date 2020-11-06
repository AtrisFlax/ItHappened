using System;
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
        private Mapper _mapper;
        private const string TableName = "TableName";

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new RequestToDomainProfile(new Utf8Coder()));
            });
            _mapper = new Mapper(mapperConfig);
        }

        [Test]
        public void DateFilterUpperAndLowerBound()
        {
            //arrange

            var eventFilterRequest = new EventFilterDataRequest
            {
                FromDateTime = new DateTime(2019, 10, 14, 18, 01, 00),
                ToDateTime = new DateTime(2020, 10, 14, 18, 00, 00)
            };
            var filterStringPredicates = _mapper.Map<EventFilterData>(eventFilterRequest);

            //act
            var filter = new MssqlEventsFilter().CreateFilterMsSqlPredicates(filterStringPredicates, TableName);

            //
            Assert.AreEqual(
                $"CAST({TableName}.HappensDate AS DATE) >= CAST('20191014 18:01:00.000' AS DATETIME) and CAST({TableName}.HappensDate AS DATE) <= CAST('20201014 18:00:00.000' AS DATETIME)",
                filter);
        }

        [Test]
        public void DateFilterUpperBound()
        {
            //arrange
            var eventFilterRequest = new EventFilterDataRequest
            {
                ToDateTime = new DateTime(2020, 10, 14, 18, 00, 00)
            };
            var filtersData = _mapper.Map<EventFilterData>(eventFilterRequest);

            //act
            var filterStringPredicates =
                new MssqlEventsFilter().CreateFilterMsSqlPredicates(filtersData, TableName);

            //
            Assert.AreEqual($"CAST({TableName}.HappensDate AS DATE) <= CAST('20201014 18:00:00.000' AS DATETIME)",
                filterStringPredicates);
        }

        [Test]
        public void DateFilterLowerBound()
        {
            //arrange
            var eventFilterRequest = new EventFilterDataRequest
            {
                FromDateTime = new DateTime(2019, 10, 14, 18, 01, 00),
            };
            var filterStringPredicates = _mapper.Map<EventFilterData>(eventFilterRequest);

            //act
            var filter = new MssqlEventsFilter().CreateFilterMsSqlPredicates(filterStringPredicates, TableName);

            //
            Assert.AreEqual($"CAST({TableName}.HappensDate AS DATE) >= CAST('20191014 18:01:00.000' AS DATETIME)",
                filter);
        }

        [Test]
        public void RatingFilterUpperAndLowerBound()
        {
            //arrange
            var eventFilterRequest = new EventFilterDataRequest
            {
                LowerLimitRating = 2,
                UpperLimitRating = 5.5
            };
            var filtersData = _mapper.Map<EventFilterData>(eventFilterRequest);

            //act
            var filterStringPredicates = new MssqlEventsFilter().CreateFilterMsSqlPredicates(filtersData, TableName);

            //assert
            Assert.AreEqual($"{TableName}.rating >= 2 and {TableName}.rating <= 5.5", filterStringPredicates);
        }

        [Test]
        public void RatingFilterLowerBound()
        {
            //arrange
            var eventFilterRequest = new EventFilterDataRequest
            {
                LowerLimitRating = 2,
            };
            var filtersData = _mapper.Map<EventFilterData>(eventFilterRequest);

            //act
            var filterStringPredicates =
                new MssqlEventsFilter().CreateFilterMsSqlPredicates(filtersData, TableName);

            //assert
            Assert.AreEqual($"{TableName}.rating >= 2", filterStringPredicates);
        }

        [Test]
        public void RatingFilterUpperBound()
        {
            //arrange
            var eventFilterRequest = new EventFilterDataRequest
            {
                UpperLimitRating = 5.5
            };
            var filtersData = _mapper.Map<EventFilterData>(eventFilterRequest);

            //act
            var filterStringPredicates =
                new MssqlEventsFilter().CreateFilterMsSqlPredicates(filtersData, TableName);

            //assert
            Assert.AreEqual(filterStringPredicates, $"{TableName}.rating <= 5.5");
        }

        [Test]
        public void ScaleFilterUpperAndLowerBound()
        {
            //arrange
            var eventFilterRequest = new EventFilterDataRequest
            {
                ScaleLowerLimit = 2,
                ScaleUpperLimit = 5.5
            };
            var filtersData = _mapper.Map<EventFilterData>(eventFilterRequest);

            //act
            var filterStringPredicates =
                new MssqlEventsFilter().CreateFilterMsSqlPredicates(filtersData, TableName);

            //assert
            Assert.AreEqual($"{TableName}.scale >= 2 and {TableName}.scale <= 5.5", filterStringPredicates);
        }

        [Test]
        public void ScaleFilterLowerBound()
        {
            //arrange
            var eventFilterRequest = new EventFilterDataRequest
            {
                ScaleLowerLimit = 2,
            };
            var filtersData = _mapper.Map<EventFilterData>(eventFilterRequest);

            //act
            var filterStringPredicates = new MssqlEventsFilter().CreateFilterMsSqlPredicates(filtersData, TableName);

            //assert
            Assert.AreEqual($"{TableName}.scale >= 2", filterStringPredicates);
        }

        [Test]
        public void ScaleFilterUpperBound()
        {
            //arrange
            var eventFilterRequest = new EventFilterDataRequest
            {
                ScaleUpperLimit = 5.5
            };
            var filtersData = _mapper.Map<EventFilterData>(eventFilterRequest);

            //act
            var filterStringPredicates =
                new MssqlEventsFilter().CreateFilterMsSqlPredicates(filtersData, TableName);

            //assert
            Assert.AreEqual($"{TableName}.scale <= 5.5", filterStringPredicates);
        }

        [Test]
        public void GetTagBounds()
        {
            //arrange
            var eventFilterRequest = new EventFilterDataRequest
            {
                GpsLatLeftCorner = 10.0,
                GpsLngLeftCorner = 11.0,
                GpsLatRightCorner = 20.0,
                GpsLngRightCorner = 21.0
            };
            var filtersData = _mapper.Map<EventFilterData>(eventFilterRequest);

            //act
            var filterStringPredicates = new MssqlEventsFilter().CreateFilterMsSqlPredicates(filtersData, TableName);

            //assert
            Assert.AreEqual(
                $"{TableName}.latitudeGeo >= 10 and {TableName}.latitudeGeo <= 20 and {TableName}.longitudeGeo >= 11 and {TableName}.longitudeGeo <= 21",
                filterStringPredicates);
        }

        //TODO mix Lat\Lng and Left\Right with different skip in eventFilterRequest


        [Test]
        public void Comment()
        {
            //arrange
            var eventFilterRequest = new EventFilterDataRequest
            {
                SubstringForMatching = "SomeComment",
            };
            var filtersData = _mapper.Map<EventFilterData>(eventFilterRequest);

            //act
            var filterStringPredicates = new MssqlEventsFilter().CreateFilterMsSqlPredicates(filtersData, TableName);

            //assert
            Assert.AreEqual($"{TableName}.comment LIKE '%SomeComment%'", filterStringPredicates);
        }

        [Test]
        public void AllPredicates()
        {
            //arrange
            var eventFilterRequest = new EventFilterDataRequest
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
            var filtersData = _mapper.Map<EventFilterData>(eventFilterRequest);

            //act
            var filterStringPredicates =
                new MssqlEventsFilter().CreateFilterMsSqlPredicates(filtersData, TableName);

            //assert
            Assert.AreEqual(
                $@"CAST({TableName}.HappensDate AS DATE) >= CAST('20201014 18:01:00.000' AS DATETIME) 
and CAST({TableName}.HappensDate AS DATE) <= CAST('20191014 18:00:00.000' AS DATETIME) and {TableName}.scale >= 0.3 
and {TableName}.scale <= 400 and {TableName}.rating >= 0.2 and {TableName}.rating <= 0.5 and {TableName}.comment LIKE '%aaa%' 
and {TableName}.latitudeGeo >= 15.8 and {TableName}.latitudeGeo <= 32.4 and {TableName}.longitudeGeo >= 17.9 
and {TableName}.longitudeGeo <= 78.4".Replace("\r\n", ""),
                filterStringPredicates);
        }
    }
}