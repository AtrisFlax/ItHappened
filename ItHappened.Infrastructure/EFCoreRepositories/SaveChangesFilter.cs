using Microsoft.AspNetCore.Mvc.Filters;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class SaveChangesFilter : ActionFilterAttribute
    {
        private readonly ItHappenedDbContext _dbContext;

        public SaveChangesFilter(ItHappenedDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                _dbContext.SaveChanges();
            }
        }
    }
}