using GameProject.Models;
using GameProject.Models.Entities;
using GameProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameProject.Areas.Admin.Controllers
{
    public class WeaponController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            Weapon weapon = new Weapon()
            {
                Type = WeaponType.OneHanded,
                Name = "Ala Ma Kota",
                MinDamage = 1,
                MaxDamage = 3,
                QualityLevel = 1,
                Price = 1
            };

            db.Weapons.Add(weapon);
            db.SaveChanges();

            var weapons = from w in db.Weapons
                          select w;

            return View(weapons.ToList());
        }
	}
}
