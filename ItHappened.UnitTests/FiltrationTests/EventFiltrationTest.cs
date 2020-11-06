using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using AutoMapper;
using Dapper;
using ItHappened.Api.Mapping;
using ItHappened.Api.Models.Requests;
using ItHappened.Domain;
using ItHappened.Infrastructure;
using LanguageExt;
using Moq;
using NUnit.Framework;
using Moq.Dapper;

namespace ItHappened.UnitTests.FiltrationTests
{
    public class EventFiltrationTest
    {
        [Test]
        public void EventFiltration()
        {
            //arrange
            var dbConnectionMock = new Mock<IDbConnection>();
            var dbTransaction = new Mock<DbTransaction>();
            var trackerId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var events = new List<Event>
            {
                new Event(Guid.NewGuid(),
                    userId,
                    trackerId,
                    DateTimeOffset.UtcNow,
                    new EventCustomParameters(
                        Option<Photo>.None,
                        Option<double>.Some(1.0),
                        Option<double>.None,
                        Option<GeoTag>.None,
                        Option<Comment>.None)),
                new Event(Guid.NewGuid(),
                    userId,
                    trackerId,
                    DateTimeOffset.UtcNow,
                    new EventCustomParameters(
                        Option<Photo>.None,
                        Option<double>.Some(11.0),
                        Option<double>.None,
                        Option<GeoTag>.None,
                        Option<Comment>.None)),
            };
            dbConnectionMock.SetupDapper(c =>
                    c.Query<Event>(
                        It.IsAny<string>(),
                        null,
                        null,
                        true,
                        null,
                        null))
                .Returns(events);


            var eventFilterRequest = new EventFilterDataRequest
            {
                FromDateTime = new DateTime(2020, 10, 14, 18, 01, 00),
                ToDateTime = new DateTime(2019, 10, 14, 18, 00, 00),
                LowerLimitRating = 0.5,
                UpperLimitRating = 0.5,
                SubstringForMatching = "aaa",
                ScaleLowerLimit = 0.3,
                ScaleUpperLimit = 0.2,
                GpsLatLeftCorner = 15.8,
                GpsLngLeftCorner = 17.9,
                GpsLatRightCorner = 32.4,
                GpsLngRightCorner = 78.4
            };
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new RequestToDomainProfile( new Utf8Coder())); });
            var mapper = new Mapper(mapperConfig);
            var filtersData = mapper.Map<EventFilterData>(eventFilterRequest);
            var filter = new MssqlEventsFilter();

            //act
            // var eventFiltration = new EventFiltration(dbConnectionMock.Object, dbTransaction.Object, filter);
            //useless moq. Always get arranged events
            // var filteredEventsResult =
                // eventFiltration.GetAllFilteredEvents(Guid.NewGuid(), Guid.NewGuid(), filtersData);


            //assert
            //Object Event not fully  constructed. When event have private parameterless constructor field EventCustomParameters is null
            
        }
    }
}