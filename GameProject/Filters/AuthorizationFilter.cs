using GameProject.Enums;
using GameProject.Models;
using GameProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GameProject.Filters
{
    public class AuthorizationFilter : ActionFilterAttribute, IActionFilter, IDisposable
    {
        private DatabaseContext db = new DatabaseContext();
        private UserRole userRole;

        public AuthorizationFilter(UserRole userRole)
        {
            this.userRole = userRole;
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserSessionContext us = new UserSessionContext(filterContext.HttpContext);

            int userId = us.GetUserId();

            var query = from u in db.Users
                        where u.Id == userId && u.Role == userRole
                        select u;

            var user = query.FirstOrDefault();

            if (user == null)
            {
                RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("action", "Login");
                redirectTargetDictionary.Add("controller", "Account");

                filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
            }
            else
            {
                filterContext.HttpContext.Items.Add("User", user);
            }

            this.OnActionExecuting(filterContext);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
