using GameProject.Enums;
using GameProject.Models;
using GameProject.Models.Entities;
using GameProject.Services;
using GameProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GameProject.Filters
{
    public class CharacterResourcesFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(!filterContext.HttpContext.Items.Contains("Character"))
            {
                this.OnActionExecuting(filterContext);
                return;
            }

            Character character = filterContext.HttpContext.Items["Character"] as Character;

            DateTime timeNow = DateTime.Now;

            double differentSeconds = (timeNow - character.RenewalTime).TotalSeconds;

            if ((character.AvailableMoves < 10) &&
                (differentSeconds >= 30))
            {
                character.RenewalTime = timeNow;

                int moves = (int)(differentSeconds / 30);
                character.AvailableMoves += moves <= 10 ? moves : 10;

                using (DatabaseContext db = new DatabaseContext())
                {
                    db.Entry(character).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            CharacterResourcesViewModel characterResourcesViewModel = new CharacterResourcesViewModel()
            {
                Gold = character.Gold,
                AvailableMoves = character.AvailableMoves,
                AvailableMovesReduction = 10
            };

            filterContext.HttpContext.Items.Add("CharacterResources", characterResourcesViewModel);

            this.OnActionExecuting(filterContext);
        }
    }
}
