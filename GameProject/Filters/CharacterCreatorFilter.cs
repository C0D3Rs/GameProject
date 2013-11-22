using GameProject.Enums;
using GameProject.Models;
using GameProject.Models.Entities;
using GameProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GameProject.Filters
{
    public class CharacterCreatorFilter : ActionFilterAttribute, IActionFilter
    {
        private DatabaseContext db = new DatabaseContext();
        private UserSessionContext us = new UserSessionContext();

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(!filterContext.HttpContext.Items.Contains("User"))
            {
                this.OnActionExecuting(filterContext);
                return;
            }

            User user = filterContext.HttpContext.Items["User"] as User;

            var query = from c in db.Characters
                        where c.UserId == user.Id
                        select c;

            var character = query.FirstOrDefault();

            if (character == null)
            {
                RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("action", "Create");
                redirectTargetDictionary.Add("controller", "Character");

                filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
            }
            else
            {
                filterContext.HttpContext.Items.Add("Character", character);
            }

            this.OnActionExecuting(filterContext);
        }
    }
}
