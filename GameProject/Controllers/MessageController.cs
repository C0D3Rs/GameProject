using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameProject.Services;
using GameProject.ViewModels;
using GameProject.Models.Entities;

namespace GameProject.Controllers
{
    public class MessageController : Controller
    {
        //
        // GET: /Message/
        public ActionResult Index()
        {
            Monster monster = new Monster();

            monster.Name = "Smok";
            monster.Level = 5;
            monster.ChanceToHit = 35;
            monster.AttackSpeed = 1.25M;
            monster.MinDamage = 2;
            monster.MaxDamage = 7;
            monster.Defense = 10;
            monster.Life = 20;

            CharacterService characterService = new CharacterService();
            CharacterViewModel characterViewModel = new CharacterViewModel();

            characterViewModel.Name = "Lukii";
            characterViewModel.Level = 6;
            characterViewModel.Life = 25;
            characterViewModel.MinDamage = 1;
            characterViewModel.MaxDamage = 5;
            characterViewModel.Armor = 10;
            characterViewModel.AttackSpeed = 1.5M;
            characterViewModel.ChanceToHit = 85;

            bool Winner = true;
            string GetMoreDetails = characterService.GetBattleReport(characterViewModel, monster, ref Winner);
            return View(model: GetMoreDetails);
        }
    }
}