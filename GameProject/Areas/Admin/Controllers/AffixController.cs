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
using GameProject.Services;
using GameProject.Helpers;
using System.Data.Entity.Infrastructure;
using GameProject.Filters;

namespace GameProject.Areas.Admin.Controllers
{
    [AuthorizationFilter(UserRole.Admin)]
    public class AffixController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var query = from a in db.Affixes
                        orderby a.Id descending
                        select a;

            return View(query.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from a in db.Affixes
                        where a.Id == id
                        select a;

            var affix = query.FirstOrDefault();

            if (affix == null)
            {
                return HttpNotFound();
            }

            return View(affix);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Affix affix)
        {
            if (!Enum.IsDefined(typeof(AffixType), affix.Type))
            {
                ModelState.AddModelError("Type", new Exception());
            }

            try
            {
                if (ModelState.IsValid)
                {
                    db.Affixes.Add(affix);
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

            return View(affix);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from a in db.Affixes
                        where a.Id == id
                        select a;

            var affix = query.FirstOrDefault();

            if (affix == null)
            {
                return HttpNotFound();
            }

            return View(affix);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Affix affix)
        {
            if (!Enum.IsDefined(typeof(AffixType), affix.Type))
            {
                ModelState.AddModelError("Type", new Exception());
            }

            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(affix).State = EntityState.Modified;
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

            return View(affix);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from a in db.Affixes
                        where a.Id == id
                        select a;

            var affix = query.FirstOrDefault();

            if (affix == null)
            {
                return HttpNotFound();
            }

            try
            {
                db.Affixes.Remove(affix);
                db.SaveChanges();

                FlashMessageHelper.SetMessage(this, FlashMessageType.Success, "Usunięcie danych przebiegło pomyślnie.");
            }
            catch (Exception)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Wystąpił nieoczekiwany błąd związany z usuwaniem danych.");
            }

            return RedirectToAction("Index");
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
