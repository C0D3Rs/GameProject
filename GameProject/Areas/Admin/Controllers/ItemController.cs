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
    public class ItemController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(int? page)
        {
            var query = from i in db.Items
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

            var query = from i in db.Items
                        where i.Id == id
                        select i;

            var item = query.FirstOrDefault();

            if (item == null)
            {
                return HttpNotFound();
            }

            DetailsItemViewModel displayItemViewModel = new DetailsItemViewModel();

            displayItemViewModel.Item = item;

            var query2 = from i in db.Images
                         where i.ID == item.ImageId
                         select i;

            var image = query2.FirstOrDefault();

            if (image != null)
            {
                displayItemViewModel.Image = image;
            }

            if (item.Type == ItemType.Weapon)
            {
                return View("DetailsWeapon", displayItemViewModel);
            }
            else if (item.Type == ItemType.Shield)
            {
                return View("DetailsShield", displayItemViewModel);
            }
            else if (item.Type == ItemType.Armor)
            {
                return View("DetailsArmor", displayItemViewModel);
            }
            else if (item.Type == ItemType.Jewelry)
            {
                return View("DetailsJewelry", displayItemViewModel);
            }
            return HttpNotFound();
        }

        public ActionResult CreateWeapon()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWeapon(WeaponItemViewModel weaponItemViewModel)
        {
            if (!Enum.IsDefined(typeof(WeaponType), (WeaponType)weaponItemViewModel.Type))
            {
                ModelState.AddModelError("Type", new Exception());
            }

            Item item = new Item()
            {
                Type = ItemType.Weapon,
                SubType = (SubType)weaponItemViewModel.Type,
                Name = weaponItemViewModel.Name,
                Durability = weaponItemViewModel.Durability,
                Price = weaponItemViewModel.Price,
                PrimaryMinValue = weaponItemViewModel.MinDamage,
                PrimaryMaxValue = weaponItemViewModel.MaxDamage,
                QualityLevel = weaponItemViewModel.QualityLevel,
                RequireStrength = weaponItemViewModel.RequireStrength,
            };

            try
            {
                if (ModelState.IsValid)
                {
                    db.Items.Add(item);
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

            return View(weaponItemViewModel);
        }

        public ActionResult CreateShield()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateShield(ShieldItemViewModel shieldItemViewModel)
        {
            Item item = new Item()
            {
                Type = ItemType.Shield,
                Name = shieldItemViewModel.Name,
                Durability = shieldItemViewModel.Durability,
                Price = shieldItemViewModel.Price,
                PrimaryMinValue = shieldItemViewModel.Armor,
                PrimaryMaxValue = shieldItemViewModel.Armor,
                QualityLevel = shieldItemViewModel.QualityLevel,
                RequireStrength = shieldItemViewModel.RequireStrength,
            };

            try
            {
                if (ModelState.IsValid)
                {
                    db.Items.Add(item);
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

            return View(shieldItemViewModel);
        }

        public ActionResult CreateArmor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateArmor(ShieldItemViewModel armorItemViewModel)
        {
            Item item = new Item()
            {
                Type = ItemType.Armor,
                Name = armorItemViewModel.Name,
                Durability = armorItemViewModel.Durability,
                Price = armorItemViewModel.Price,
                PrimaryMinValue = armorItemViewModel.Armor,
                PrimaryMaxValue = armorItemViewModel.Armor,
                QualityLevel = armorItemViewModel.QualityLevel,
                RequireStrength = armorItemViewModel.RequireStrength,
            };

            try
            {
                if (ModelState.IsValid)
                {
                    db.Items.Add(item);
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

            return View(armorItemViewModel);
        }

        public ActionResult CreateJewelry()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateJewelry(JewelryItemViewModel jewelryItemViewModel)
        {
            if (!Enum.IsDefined(typeof(JewelryType), (JewelryType)jewelryItemViewModel.Type))
            {
                ModelState.AddModelError("Type", new Exception());
            }

            Item item = new Item()
            {
                Type = ItemType.Jewelry,
                SubType = (SubType)jewelryItemViewModel.Type,
                Name = jewelryItemViewModel.Name,
                Durability = jewelryItemViewModel.Durability,
                Price = jewelryItemViewModel.Price,
                QualityLevel = jewelryItemViewModel.QualityLevel,
            };

            try
            {
                if (ModelState.IsValid)
                {
                    db.Items.Add(item);
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

            return View(jewelryItemViewModel);
        }

        private ActionResult GetEditItemView(Item item)
        {
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

            return GetEditItemView(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (!Enum.IsDefined(typeof(ItemType), item.Type))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if ((item.Type == ItemType.Weapon &&
                !Enum.IsDefined(typeof(WeaponType), (WeaponType)item.SubType)) ||
                (item.Type == ItemType.Jewelry && !Enum.IsDefined(typeof(JewelryType), (JewelryType)item.SubType)))
            {
                ModelState.AddModelError("SubType", new Exception());
            }

            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();

                    FlashMessageHelper.SetMessage(this, FlashMessageType.Success, "Aktualizacja danych przebiegła pomyślnie.");
                    return RedirectToAction("Index");
                }

                FlashMessageHelper.SetMessage(this, FlashMessageType.Info, "Nie można zaktualizować danych. Należy poprawić zaistniałe błędy.");
            }
            catch (DbUpdateConcurrencyException)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Warning, "Dane zostały zaktualizowane przez inną osobę. Należy odświeżyć stronę w celu wczytania nowych danych.");
            }
            catch (Exception)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Wystąpił nieoczekiwany błąd związany z aktualizowaniem danych.");
            }

            return GetEditItemView(item);
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

            try
            {
                db.Items.Remove(item);
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
