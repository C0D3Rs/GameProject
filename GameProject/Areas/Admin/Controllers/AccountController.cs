using GameProject.Models;
using GameProject.Models.Entities;
using GameProject.Areas.Admin.ViewModels;
using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameProject.Helpers;
using GameProject.Filters;
using GameProject.Services;

namespace GameProject.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var query = from u in db.Users
                        where u.Name == model.Username && u.Role == UserRole.Admin
                        select u;

            var user = query.FirstOrDefault();

            if (user == null || !PasswordHashService.ValidatePassword(model.Password, user.Password))
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Warning, "Autoryzacja danych nie przebiegła pomyślnie.");
                return View(model);
            }

            UserSessionContext us = new UserSessionContext(HttpContext);
            us.SetUserId(user.Id);

            return RedirectToAction("Index", "Dashboard");
        }

        [AuthorizationFilter(UserRole.Admin)]
        public ActionResult Logout()
        {
            UserSessionContext us = new UserSessionContext(HttpContext);
            us.RemoveUserId();

            return RedirectToAction("Login", "Account");
        }
    }
}
