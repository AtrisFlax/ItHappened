using System;
using System.Collections.Generic;
using System.Data;
using AutoMapper;
using Dapper;
using ItHappened.Domain;
using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure
{
    public class EventFiltration : IEventFilterable
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        private readonly IMssqlFilter _mssqlFilter;
        private readonly IMapper _mapper;

        private const string SchemaName = "ItHappenedDB";
        private const string TableName = "Events";

        private static readonly string SchemaAndTableName = $"{SchemaName}.{TableName}";

        public EventFiltration(IDbConnection connection, IDbTransaction transaction, IMssqlFilter mssqlFilter,
            IMapper mapper)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            _mssqlFilter = mssqlFilter;
            _mapper = mapper;
        }


        public IReadOnlyCollection<Event> GetAllFilteredEvents(Guid actorId, Guid trackerId, EventFilterData eventFilterData)
        {
            var generalPredicate = _mssqlFilter.CreateFilterMsSqlPredicates(eventFilterData, TableName);
            var events = _connection
                .Query<EventDto>(
                    @"select * from ItHappenedDB.Events as Events where Events.CreatorId = @ActorId and Events.TrackerId = @TrackerId @GeneralPredicate",
                    new
                    {
                        SqlSchemaAndTableName = SchemaAndTableName,
                        ActorId = actorId,
                        TrackerId = trackerId,
                        GeneralPredicate = generalPredicate != string.Empty ? $"and {generalPredicate}" : string.Empty
                    },
                    _transaction
                );
            return _mapper.Map<Event[]>(events);
        }
    }
}