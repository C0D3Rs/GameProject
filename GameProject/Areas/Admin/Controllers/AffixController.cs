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

namespace GameProject.Areas.Admin.Controllers
{
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

                    FlashMessageHelper.SetMessage(this, FlashMessageType.Success, Resources.Resources.FlashMessageCreateSuccess);
                    return RedirectToAction("Index");
                }
                FlashMessageHelper.SetMessage(this, FlashMessageType.Info, Resources.Resources.FlashMessageCreateInfo);
            }
            catch (Exception)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, Resources.Resources.FlashMessageCreateError);
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

                    FlashMessageHelper.SetMessage(this, FlashMessageType.Success, Resources.Resources.FlashMessageEditSuccess);
                    return RedirectToAction("Index");
                }
                FlashMessageHelper.SetMessage(this, FlashMessageType.Info, Resources.Resources.FlashMessageEditInfo);
            }
            catch (DbUpdateConcurrencyException)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Warning, Resources.Resources.FlashMessageConcurrencyWarning);
            }
            catch (Exception)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, Resources.Resources.FlashMessageEditError);
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

            return View(affix);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
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

                FlashMessageHelper.SetMessage(this, FlashMessageType.Success, Resources.Resources.FlashMessageDeleteSucces);
                return RedirectToAction("Index");
            }
            catch(Exception)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, Resources.Resources.FlashMessageDeleteError);
            }

            return View(affix);
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
