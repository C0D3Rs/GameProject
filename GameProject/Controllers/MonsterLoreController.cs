using GameProject.Enums;
using GameProject.Filters;
using GameProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameProject.Controllers
{
    [AuthorizationFilter(UserRole.Normal, Order = 1)]
    [CharacterCreatorFilter(Order = 2)]
    [CharacterResourcesFilter(Order = 3)]
    [EventFilter(Order = 4)]
    public class MonsterLoreController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var query = from m in db.Monsters
                        from el in db.EventLogs
                        from e in db.Events
                        where el.IsCompleted == true && el.EventId == e.Id && e.MonsterId == m.Id
                        orderby m.Id descending
                        select m;

            return View(query.ToList());
        }
	}
}
