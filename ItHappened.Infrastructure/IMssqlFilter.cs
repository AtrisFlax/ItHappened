using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Infrastructure
{
    public interface IMssqlFilter
    {
        public IEnumerable<string> CreateFilterMsSqlPredicates(EventFilter filterData);
    }
}