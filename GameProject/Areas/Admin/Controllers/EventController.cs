using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameProject.Models.Entities;
using GameProject.Models;
using GameProject.Enums;
using GameProject.Helpers;
using System.Data.Entity.Infrastructure;
using GameProject.Areas.Admin.ViewModels;
using GameProject.Filters;
using PagedList;

namespace GameProject.Areas.Admin.Controllers
{
    [AuthorizationFilter(UserRole.Admin)]
    public class EventController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(int? page)
        {
            var query = from i in db.Events
                        orderby i.Id descending
                        select i;

            int pageNumber = (page ?? 1);
            return View(query.ToPagedList(pageNumber, 10));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from a in db.Locations
                         from b in db.Events
                         where b.Id == id && a.ID == b.LocationId
                         join c in db.Monsters on b.MonsterId equals c.Id into C
                         from m in C.DefaultIfEmpty()
                         join i1 in db.Images on a.ImageId equals i1.ID into I1
                         from ia in I1.DefaultIfEmpty()
                         join i2 in db.Images on m.ImageId equals i2.ID into I2
                         from im in I2.DefaultIfEmpty()
                         select new DetailsEventViewModel
                         {
                             Event = b,
                             Location = a,
                             LocationImage = ia != null ? ia : null,
                             Monster = m != null ? m : null,
                             MonsterImage = im != null ? im : null
                         };

            var oneEvent = query.FirstOrDefault();

            if (oneEvent == null)
            {
                return HttpNotFound();
            }

            return View(oneEvent); 
        }
        
        public ActionResult Create(int? locationID)
        {
            if (locationID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var Location = db.Locations.Find(locationID);

            if (Location == null)
            {
                return HttpNotFound();
            }

            Event one_event = new Event();

            one_event.LocationId = Location.ID;

            var query2 = from a in db.Monsters
                         from b in db.Images
                         where a.ImageId == b.ID
                         select new DetailsMonsterViewModel
                         {
                            Monster = a,
                            Image = b
                         };
                        
            var monsters = query2.ToList();

            CreateEventViewModel model = new CreateEventViewModel();
            
            model.Location = Location;
            model.Event = one_event;
            model.Monsters = monsters;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Event one_event)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Events.Add(one_event);
                    db.SaveChanges();

                    FlashMessageHelper.SetMessage(this, FlashMessageType.Success, "Zapisanie nowych danych przebiegło pomyślnie.");
                    return RedirectToAction("Index");
                }
                FlashMessageHelper.SetMessage(this, FlashMessageType.Info, "Nie można zapisać nowych danych. Należy poprawić zaistniałe błędy.");
            }
            catch (Exception)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Wystąpił nieoczekiwany błąd związany z zapisem nowych danych.");
            }

            var query = from a in db.Locations
                        where a.ID == one_event.LocationId
                        select a;

            var fail_event = query.FirstOrDefault();


            CreateEventViewModel setItemViewModel = new CreateEventViewModel()
            {
                Location = fail_event,
                Event = one_event
            };

            return View(setItemViewModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Events
                        where i.Id == id
                        select i;

            var oneEvent = query.FirstOrDefault();

            if (oneEvent == null)
            {
                return HttpNotFound();
            }

            try
            {
                db.Events.Remove(oneEvent);
                db.SaveChanges();

                FlashMessageHelper.SetMessage(this, FlashMessageType.Success, "Usunięcie danych przebiegło pomyślnie.");
            }
            catch (Exception)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Wystąpił nieoczekiwany błąd związany z usuwaniem danych.");
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var oneEvent = db.Events.Find(id);

            if (oneEvent == null)
            {
                return HttpNotFound();
            }

            var query2 = from a in db.Monsters
                         from b in db.Images
                         where a.ImageId == b.ID
                         select new DetailsMonsterViewModel
                         {
                             Monster = a,
                             Image = b
                         };
            var Location = db.Locations.Find(oneEvent.LocationId);

            var monsters = query2.ToList();

            CreateEventViewModel model = new CreateEventViewModel();

            model.Location = Location;
            model.Event = oneEvent;
            model.Monsters = monsters;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Event oneEvent)
        {
            var query = from i in db.Events
                        where i.Id == oneEvent.Id
                        select i;

            if (!query.Any())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(oneEvent).State = EntityState.Modified;
                    db.SaveChanges();

                    FlashMessageHelper.SetMessage(this, FlashMessageType.Success, "Aktualizacja danych przebiegła pomyślnie.");
                    return RedirectToAction("Index");
                }

                FlashMessageHelper.SetMessage(this, FlashMessageType.Info, "Nie można zaktualizować danych. Należy poprawić zaistniałe błędy.");
            }
            catch (DbUpdateConcurrencyException)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Warning, "Dane został zaktualizowane przez inną osobę. Należy odświeżyć stronę w celu wczytania nowych danych.");
            }
            catch (Exception)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Wystąpił nieoczekiwany błąd związany z aktualizowaniem danych.");
            }

            var query2 = from a in db.Locations
                        where a.ID == oneEvent.LocationId
                        select a;

            var fail_event = query2.FirstOrDefault();


            CreateEventViewModel setItemViewModel = new CreateEventViewModel()
            {
                Location = fail_event,
                Event = oneEvent
            };

            return View(setItemViewModel);
        }
    }
}
