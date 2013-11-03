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

namespace GameProject.Areas.Admin.Controllers
{
    public class ItemController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var query = from i in db.Items
                        orderby i.Id descending
                        select i;

            return View(query.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Items
                        where i.Id == id
                        select i;

            var item = query.FirstOrDefault();

            if (item == null)
            {
                return HttpNotFound();
            }

            if (item.Type == ItemType.Weapon)
            {
                return View("DetailsWeapon", item);
            }
            else if (item.Type == ItemType.Shield)
            {
                return View("DetailsShield", item);
            }
            else if (item.Type == ItemType.Armor)
            {
                return View("DetailsArmor", item);
            }
            else if (item.Type == ItemType.Jewelry)
            {
                return View("DetailsJewelry", item);
            }
            return HttpNotFound();
        }

        public ActionResult Create(ItemType? type)
        {
            if (type == null || !Enum.IsDefined(typeof(ItemType), type))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if (type == ItemType.Weapon)
            {
                return View("CreateWeapon", new Item() { Type = ItemType.Weapon });
            }
            else if (type == ItemType.Shield)
            {
                return View("CreateShield", new Item() { Type = ItemType.Shield });
            }
            else if (type == ItemType.Armor)
            {
                return View("CreateArmor", new Item() { Type = ItemType.Armor });
            }
            else if (type == ItemType.Jewelry)
            {
                return View("CreateJewelry", new Item() { Type = ItemType.Jewelry });
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item)
        {
            if (!Enum.IsDefined(typeof(ItemType), item.Type))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (item.Type == ItemType.Weapon && !Enum.IsDefined(typeof(WeaponType), item.WeaponType))
            {
                ModelState.AddModelError("WeaponType", new Exception());
            }
            else if (item.Type == ItemType.Jewelry && !Enum.IsDefined(typeof(JewelryType), item.JewelryType))
            {
                ModelState.AddModelError("JewelryType", new Exception());
            }

            try
            {
                if (ModelState.IsValid)
                {
                    db.Items.Add(item);
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


            if (item.Type == ItemType.Weapon)
            {
                return View("CreateWeapon", item);
            }
            else if (item.Type == ItemType.Shield)
            {
                return View("CreateShield", item);
            }
            else if (item.Type == ItemType.Armor)
            {
                return View("CreateArmor", item);
            }
            else if (item.Type == ItemType.Jewelry)
            {
                return View("CreateJewelry", item);
            }
            return HttpNotFound();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Items
                        where i.Id == id
                        select i;

            var item = query.FirstOrDefault();

            if (item == null)
            {
                return HttpNotFound();
            }

            if (item.Type == ItemType.Weapon)
            {
                return View("EditWeapon", item);
            }
            else if (item.Type == ItemType.Shield)
            {
                return View("EditShield", item);
            }
            else if (item.Type == ItemType.Armor)
            {
                return View("EditArmor", item);
            }
            else if (item.Type == ItemType.Jewelry)
            {
                return View("EditJewelry", item);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (!Enum.IsDefined(typeof(ItemType), item.Type))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (item.Type == ItemType.Weapon && !Enum.IsDefined(typeof(WeaponType), item.WeaponType))
            {
                ModelState.AddModelError("WeaponType", new Exception());
            }
            else if (item.Type == ItemType.Jewelry && !Enum.IsDefined(typeof(JewelryType), item.JewelryType))
            {
                ModelState.AddModelError("JewelryType", new Exception());
            }

            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(item).State = EntityState.Modified;
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

            if (item.Type == ItemType.Weapon)
            {
                return View("EditWeapon", item);
            }
            else if (item.Type == ItemType.Shield)
            {
                return View("EditShield", item);
            }
            else if (item.Type == ItemType.Armor)
            {
                return View("EditArmor", item);
            }
            else if (item.Type == ItemType.Jewelry)
            {
                return View("EditJewelry", item);
            }
            return HttpNotFound();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Items
                        where i.Id == id
                        select i;

            var item = query.FirstOrDefault();

            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Items
                        where i.Id == id
                        select i;

            var item = query.FirstOrDefault();

            if (item == null)
            {
                return HttpNotFound();
            }

            try
            {
                db.Items.Remove(item);
                db.SaveChanges();

                FlashMessageHelper.SetMessage(this, FlashMessageType.Success, Resources.Resources.FlashMessageDeleteSucces);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, Resources.Resources.FlashMessageDeleteError);
            }

            return View(item);
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
