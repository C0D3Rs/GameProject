﻿using GameProject.Enums;
using GameProject.Filters;
using GameProject.Models;
using GameProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameProject.Controllers
{
    [AuthorizationFilter(UserRole.Normal, Order = 1)]
    [CharacterCreatorFilter(Order = 2)]
    [EventFilter(Order = 3)]
    [CharacterResourcesFilter(Order = 4)]
    public class MonsterLoreController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var query = from m in db.Monsters
                        from el in db.EventLogs
                        from e in db.Events
                        from i in db.Images
                        where el.IsCompleted == true && el.EventId == e.Id && e.Type == EventType.Monster && e.MonsterId == m.Id && m.ImageId == i.ID
                        orderby m.Id descending
                        select new DetailsMonsterViewModel
                        {
                            Monster = m,
                            Image = i
                        };

            return View(query.Distinct().ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var query = from m in db.Monsters
                        from el in db.EventLogs
                        from e in db.Events
                        from i in db.Images
                        where m.Id == id && el.IsCompleted == true && el.EventId == e.Id && e.Type == EventType.Monster && e.MonsterId == m.Id && m.ImageId == i.ID
                        orderby m.Id descending
                        select new DetailsMonsterViewModel
                        {
                            Monster = m,
                            Image = i
                        };

            var monster = query.FirstOrDefault();

            return View(monster);
        }
	}
}
