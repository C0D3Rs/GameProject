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
using GameProject.Helpers;
using GameProject.Enums;
using System.Data.Entity.Infrastructure;

namespace GameProject.Areas.Admin.Controllers
{
    public class MonsterController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var query = from m in db.Monsters
                        orderby m.Id descending
                        select m;

            return View(query.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from m in db.Monsters
                        where m.Id == id
                        select m;

            var monster = query.FirstOrDefault();

            if (monster == null)
            {
                return HttpNotFound();
            }

            return View(monster);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Monster monster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Monsters.Add(monster);
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

            return View(monster);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from m in db.Monsters
                        where m.Id == id
                        select m;

            var monster = query.FirstOrDefault();

            if (monster == null)
            {
                return HttpNotFound();
            }

            return View(monster);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Monster monster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(monster).State = EntityState.Modified;
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

            return View(monster);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from m in db.Monsters
                        where m.Id == id
                        select m;

            var monster = query.FirstOrDefault();

            if (monster == null)
            {
                return HttpNotFound();
            }

            try
            {
                db.Monsters.Remove(monster);
                db.SaveChanges();

                FlashMessageHelper.SetMessage(this, FlashMessageType.Success, "Usunięcie danych przebiegło pomyślnie.");
            }
            catch (Exception)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Wystąpił nieoczekiwany błąd związany z usuwaniem danych.");
            }

            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from m in db.Monsters
                        where m.Id == id
                        select m;

            var monster = query.FirstOrDefault();

            if (monster == null)
            {
                return HttpNotFound();
            }



            return View(monster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
