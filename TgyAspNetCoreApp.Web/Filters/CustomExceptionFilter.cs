using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using TgyAspNetCoreApp.Web.Models;

namespace TgyAspNetCoreApp.Web.Filters
{
    public class CustomExceptionFilter:ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            var error = context.Exception.Message;

            context.Result =  new RedirectToActionResult("Error", "Home", new ErrorViewModel()
                {
                    Errors = new List<string>() { $"{error}" }
                });
            
        }
    }
}
