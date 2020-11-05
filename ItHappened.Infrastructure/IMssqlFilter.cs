using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Infrastructure
{
    public interface IMssqlFilter
    {
        public string CreateFilterMsSqlPredicates(EventFilter filterData, string tableName);
    }
}