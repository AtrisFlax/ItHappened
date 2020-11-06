using System.Data;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ItHappened.Infrastructure
{
    public class UnitOfWorkFilter : ActionFilterAttribute
    {
        private readonly IDbTransaction _dbTransaction;

        public UnitOfWorkFilter(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                _dbTransaction.Commit();
            }
            else
            {
                _dbTransaction.Rollback();
            }
        }
    }
}