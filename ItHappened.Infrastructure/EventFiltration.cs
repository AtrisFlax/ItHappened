using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using ItHappened.Domain;

namespace ItHappened.Infrastructure
{
    public class EventFiltration : IEventFilterable
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        private readonly IMssqlFilter _mssqlFilter;

        private const string SchemaName = "ItHappenedDB";
        private static readonly string SchemaAndTableName = $"{SchemaName}.Events";

        public EventFiltration(IDbConnection connection, IDbTransaction transaction, IMssqlFilter mssqlFilter)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            _mssqlFilter = mssqlFilter;
        }

        public IReadOnlyCollection<Event> GetAllFilteredEvents(Guid actorId, Guid trackerId, EventFilter eventFilter)
        {
            var generalPredicate = CreateGeneralFilterPredicate(eventFilter);
            var events = _connection
                .Query<Event>(
                    @"select * from @SqlSchemaAndTableName where @ActorId == CreatorId and @TrackerId == TrackerId @GeneralPredicate",
                    transaction: _transaction,
                    param: new
                    {
                        SqlSchemaAndTableName = SchemaAndTableName,
                        ActorId = actorId,
                        TrackerId = trackerId,
                        GeneralPredicate = generalPredicate
                    });
            return events.ToList();
        }

        //TODO possible SQL injection. Inside _mssqlFilters.CreateFilterMsSqlPredicates. Concat through interpolation - $"HappensDate >= {eventFilter.FromDateTime} 
        private string CreateGeneralFilterPredicate(EventFilter eventFilter)
        {
            var stringFilterPredicates = _mssqlFilter.CreateFilterMsSqlPredicates(eventFilter).ToList();
            return !stringFilterPredicates.Any() ? string.Empty : string.Join(" and ", stringFilterPredicates);
        }
    }
}