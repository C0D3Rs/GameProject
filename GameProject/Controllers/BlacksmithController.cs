using GameProject.Enums;
using GameProject.Filters;
using GameProject.Models;
using GameProject.Models.Entities;
using GameProject.Services;
using GameProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GameProject.Controllers
{
    [AuthorizationFilter(UserRole.Normal, Order = 1)]
    [CharacterCreatorFilter(Order = 2)]
    [EventFilter(Order = 3)]
    [CharacterResourcesFilter(Order = 4)]
    public class BlacksmithController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private ItemService itemService = new ItemService();

        public ActionResult Index()
        {
            Character character = this.HttpContext.Items["Character"] as Character;

            if (character == null)
            {
                return HttpNotFound();
            }

            var query = from r1 in db.GeneratedItems
                        from r2 in db.Items
                        join r3 in db.Images on r2.ImageId equals r3.ID into R3
                        from r8 in R3.DefaultIfEmpty()
                        join r4 in db.Affixes on r1.PrefixId equals r4.Id into R4
                        from r5 in R4.DefaultIfEmpty()
                        join r6 in db.Affixes on r1.SuffixId equals r6.Id into R6
                        from r7 in R6.DefaultIfEmpty()
                        where r1.CharacterId == character.Id && r1.ItemId == r2.Id
                        select new ItemViewModel
                         {
                             GeneratedItem = r1,
                             Item = r2,
                             Image = r8 != null ? r8 : null,
                             Prefix = r5 != null ? r5 : null,
                             Suffix = r7 != null ? r7 : null
                         };

            List<ItemViewModel> characterItems = query.ToList();

            return View(characterItems);
        }

        public ActionResult Repair(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Character character = this.HttpContext.Items["Character"] as Character;

            if (character == null)
            {
                return HttpNotFound();
            }

            var query = from i in db.GeneratedItems
                        where i.CharacterId == character.Id
                        select i;

            List<GeneratedItem> items = query.ToList();

            var item = items.FirstOrDefault(i => i.Id == id && i.Status != ItemStatus.Chest);

            if (item == null)
            {
                return RedirectToAction("Index");
            }

            if (items.Count(i => i.Status == ItemStatus.Chest) >= 10)
            {
                return RedirectToAction("Index");
            }

            try
            {
                item.Status = ItemStatus.Chest;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {
                // komunikat jakiś
            }

            return RedirectToAction("Index");
        }
	}
}
