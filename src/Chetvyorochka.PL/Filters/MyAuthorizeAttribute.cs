using Chetvyorochka.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Text.Json;
using System.Text;
using Microsoft.Net.Http.Headers;

namespace Chetvyorochka.PL.Filters
{
    public class MyAuthorizeAttribute : ActionFilterAttribute
    {
        public string Roles { get; set; }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
            {
                var repeated = context.HttpContext.Request.Cookies["isRepeated"];
                if (!String.IsNullOrEmpty(repeated))
                {
                    context.HttpContext.Response.Cookies.Delete("isRepeated");
                }
                return;
            }

            var user = context.HttpContext.User;

            if (user is { Identity.IsAuthenticated: true })
            {
                if (Roles != null && !user.IsInRole(Roles))
                {
                    RedirectWithRepeat(context, "Home", "Index");
                }
            }
            else
            {
                RedirectWithRepeat(context, "Login", "Index");
            }
        }

        public void RedirectWithRepeat(ActionExecutingContext context, string controller, string action)
        {
            var repeated = context.HttpContext.Request.Cookies["isRepeated"];
            if (String.IsNullOrEmpty(repeated))
            {
                context.HttpContext.Response.Cookies.Append("isRepeated", "true");
                context.Result = new RedirectToRouteResult(
                                   new RouteValueDictionary(
                                       new
                                       {
                                           action = action,
                                           controller = controller
                                       }));
            }
            else
            {
                context.HttpContext.Response.Cookies.Delete("isRepeated");
            }
        }
    }
}