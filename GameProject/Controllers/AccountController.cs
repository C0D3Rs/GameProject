using GameProject.Models;
using GameProject.Models.Entities;
using GameProject.ViewModels;
using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameProject.Helpers;
using GameProject.Filters;
using GameProject.Services;

namespace GameProject.Controllers
{
    public class AccountController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private UserSessionContext us = new UserSessionContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var query = from u in db.Users
                        where u.Name == model.Username
                        select u;

            var user = query.FirstOrDefault();

            if (user == null || !PasswordHashService.ValidatePassword(model.Password, user.Password))
            {
                return View(model);
            }

            us.SetHttpSessionStateBase(this.HttpContext.Session);
            us.SetUserId(user.Id);

            return RedirectToAction("Index", "Character");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterUserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                User user = new User()
                {
                    Role = UserRole.Normal,
                    Name = model.Username,
                    Password = PasswordHashService.CreateHash(model.Password)
                };

                db.Users.Add(user);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return View(model);
            }

            return RedirectToAction("Login");
        }

        [AuthorizationFilter(UserRole.Normal)]
        public ActionResult Logout()
        {
            us.SetHttpSessionStateBase(this.HttpContext.Session);
            us.RemoveUserId();

            return RedirectToAction("Login", "Account");
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
