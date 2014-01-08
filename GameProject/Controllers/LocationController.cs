using GameProject.ViewModels;
using GameProject.Enums;
using GameProject.Filters;
using GameProject.Helpers;
using GameProject.Models;
using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameProject.Services;

namespace GameProject.Controllers
{
    [AuthorizationFilter(UserRole.Normal, Order = 1)]
    [CharacterCreatorFilter(Order = 2)]
    [CharacterResourcesFilter(Order = 3)]
    [EventFilter(Order = 4)]
    public class LocationController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var query = from r1 in db.Locations
                        from r2 in db.Images
                        where r1.ImageId == r2.ID &&
                              r2.Category == ImageCategory.Location
                        select new DetailsLocationViewModel
                        {    Location = r1,
                             Image = r2
                        };

            return View(query.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var query = from i in db.Locations
                        where i.ID == id
                        select i;
            var location = query.FirstOrDefault();

            if (location == null)
            {
                return HttpNotFound();
            }

            DetailsLocationViewModel model = new DetailsLocationViewModel();
            model.Location = location;

            var query2 = from i in db.Images
                        where i.ID == location.ImageId
                        select i;

            var image = query2.FirstOrDefault();

            if (image != null)
            {
                model.Image = image;
            }
            else
            {
                model.Image = null;
            }

            return View(model);
        }

        public ActionResult Travel(int? locationID)
        {
            Character character = this.HttpContext.Items["Character"] as Character;

            if (locationID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (character.AvailableMoves == 0)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Nie masz wystarczającej ilości ruchów. Spróbuj za chwilę...");
                return RedirectToAction("Index");
            }

            character.AvailableMoves -= 2;

            CharacterService characterserv = new CharacterService();

            CharacterViewModel characterview = characterserv.GetCharacterViewModel(character, new List<ItemViewModel>());

            var query = from e in db.Events
                        from l in db.Locations
                        from i in db.Images
                        where l.ID == locationID && l.ImageId == i.ID && e.LocationId == locationID
                        join m in db.Monsters on e.MonsterId equals m.Id into M1
                        from m2 in M1.DefaultIfEmpty()
                        where m2.Level <= characterview.Level
                        orderby e.Id
                        select e;

            int eventCouter = query.Count();

            if (eventCouter == 0)
            {
                return HttpNotFound();
            }

            Random dice = new Random();

            int random = dice.Next(0, eventCouter);

            var winEvent = query.Skip(random).Take(1).FirstOrDefault();

            if (winEvent == null)
            {
                return HttpNotFound();
            }

            EventLog log = new EventLog();

            log.CharacterId = character.Id;
            log.EventId = winEvent.Id;
            log.Created_at = DateTime.Now;
            log.IsCompleted = false;

            try
            {
                db.EventLogs.Add(log);
                db.SaveChanges();
            }
            catch (Exception)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Wystąpił nieoczekiwany błąd związany z aktualizowaniem danych.");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
