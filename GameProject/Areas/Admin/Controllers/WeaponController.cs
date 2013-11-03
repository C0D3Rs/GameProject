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
using GameProject.Services;
using GameProject.Enums;
using GameProject.Areas.Admin.ViewModels;

namespace GameProject.Areas.Admin.Controllers
{
    public class WeaponController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private ItemService itemService = new ItemService();

        public ActionResult Index()
        {
            var query = from w in db.Weapons
                        orderby w.Id descending
                        select w;

            return View(query.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateWeaponViewModel createWeaponViewModel)
        {
            if (!Enum.IsDefined(typeof(WeaponType), createWeaponViewModel.Weapon.Type))
            {
                ModelState.AddModelError("Type", new Exception());
            }

            DbContextTransaction transaction = db.Database.BeginTransaction();

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(createWeaponViewModel);
                }

                createWeaponViewModel.Item.Type = ItemType.Weapon;
                db.Items.Add(createWeaponViewModel.Item);
                db.SaveChanges();

                createWeaponViewModel.Weapon.ItemId = createWeaponViewModel.Item.Id;
                db.Weapons.Add(createWeaponViewModel.Weapon);
                db.SaveChanges();

                transaction.Commit();

                return RedirectToAction("Index");
            }
            catch(Exception)
            {
                transaction.Rollback();
            }

            return View(createWeaponViewModel);
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
