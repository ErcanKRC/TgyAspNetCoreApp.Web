using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace TgyAspNetCoreApp.Web.Filters
{
    public class LogFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Debug.WriteLine("Before action method executed");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Debug.WriteLine("After action method executed");
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Debug.WriteLine("Before action method result executed");
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            Debug.WriteLine("After action method result executed");
        }
    }
}
