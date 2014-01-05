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
    public class EventFilter : ActionFilterAttribute, IActionFilter, IDisposable
    {
        private DatabaseContext db = new DatabaseContext();
        

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(!filterContext.HttpContext.Items.Contains("Character"))
            {
                this.OnActionExecuting(filterContext);
                return;
            }

            Character character = filterContext.HttpContext.Items["Character"] as Character;

            // zdarzenie walki - postać wygrała!

            int qualityLevel = 10;

            var query = from i in db.GeneratedItems
                        where i.CharacterId == character.Id && i.Status == ItemStatus.Bagpack
                        select i;

            if (query.Count() != 0)
            {
                return;
            }

            ItemService itemService = new ItemService();

            var item = itemService.GetRandomItem(qualityLevel);

            if (item == null)
            {
                return;
            }

            var prefix = itemService.GetRandomAffix(AffixType.Prefix, item.Type, qualityLevel);
            var suffix = itemService.GetRandomAffix(AffixType.Suffix, item.Type, qualityLevel);

            var generatedItem = itemService.GetGeneratedItem(item, prefix, suffix);

            if (generatedItem == null)
            {
                return;
            }

            try
            {
                generatedItem.Status = ItemStatus.Bagpack;
                generatedItem.CharacterId = character.Id;
                db.GeneratedItems.Add(generatedItem);
                db.SaveChanges();
            }
            catch (Exception)
            {
                // komunikat jakiś
            }
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
