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
using GameProject.Models.Enums;

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
        public ActionResult Create([Bind(Include="Type,Name,ForWeapon,ForShield,ForArmor,ForJewelry,MinStrength,MaxStrength,QualityLevel,Price")] Affix affix)
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
                    return RedirectToAction("Index");
                }
            }
            catch(Exception)
            {
                // wyświetlenie komunikatu o błędzie
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
        public ActionResult Edit([Bind(Include="Id,Type,Name,ForWeapon,ForShield,ForArmor,ForJewelry,MinStrength,MaxStrength,QualityLevel,Price")] Affix affix)
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
                    return RedirectToAction("Index");
                }
            }
            catch(Exception)
            {
                // wyświetlenie komunikatu o błędzie
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

            db.Affixes.Remove(affix);
            db.SaveChanges();

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
