using GameProject.Enums;
using GameProject.Filters;
using GameProject.Models;
using GameProject.Models.Entities;
using GameProject.Services;
using GameProject.ViewModels;
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
        private CharacterService cs = new CharacterService();

        [CharacterCreatorFilter(Order = 1)]
        [CharacterResourcesFilter(Order = 2)]
        public ActionResult Index()
        {
            Character character = this.HttpContext.Items["Character"] as Character;

            if (character == null)
            {
                return HttpNotFound();
            }

            var query = from gi in db.GeneratedItems
                        from i in db.Items
                        where gi.CharacterId == character.Id &&
                            gi.ItemId == i.Id &&
                            gi.Status == ItemStatus.Equipped
                        select new ItemViewModel
                        {
                            GeneratedItem = gi,
                            Item = i
                        };

            List<ItemViewModel> items = query.ToList();
            
            return View(cs.GetCharacterViewModel(character, items));
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
            character.RenewalTime = DateTime.Now;
            character.AvailableMoves = 0;

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
