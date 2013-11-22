﻿using GameProject.Enums;
using GameProject.Filters;
using GameProject.Models;
using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameProject.Controllers
{
    [AuthorizationFilter(UserRole.Normal)]
    public class CharacterController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private UserSessionContext us = new UserSessionContext();

        [CharacterCreatorFilter]
        public ActionResult Index()
        {
            Character character = this.HttpContext.Items["Character"] as Character;

            if (character == null)
            {
                return HttpNotFound();
            }

            return View(character);
        }

        public ActionResult Create()
        {
            User user = this.HttpContext.Items["User"] as User;
            var query = from c in db.Characters
                        where c.UserId == user.Id
                        select c;

            if(query.Any())
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Create(Character character)
        {
            User user = this.HttpContext.Items["User"] as User;
            var query = from c in db.Characters
                        where c.UserId == user.Id
                        select c;

            if (query.Any())
            {
                return RedirectToAction("Index");
            }

            character.UserId = user.Id;
            character.Experience = 0;
            character.Gold = 0;

            try
            {
                if(ModelState.IsValid)
                {
                    db.Characters.Add(character);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch(Exception)
            {

            }

            return View(character);
        }
	}
}
