﻿using GameProject.Models;
using GameProject.Models.Entities;
using GameProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameProject.Enums;
using GameProject.ViewModels;
using System.Net;
using System.Data.Entity;
using GameProject.Filters;
using GameProject.Helpers;

namespace GameProject.Controllers
{
    [AuthorizationFilter(UserRole.Normal, Order = 1)]
    [CharacterCreatorFilter(Order = 2)]
    [EventFilter(Order = 3)]
    [CharacterResourcesFilter(Order = 4)]
    public class EquipmentController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private ItemService itemService = new ItemService();
        private CharacterService cs = new CharacterService();

        public ActionResult Index()
        {
            Character character = this.HttpContext.Items["Character"] as Character;

            if (character == null)
            {
                return HttpNotFound();
            }

            var query2 = from r1 in db.GeneratedItems
                         from r2 in db.Items
                         join r3 in db.Images on r2.ImageId equals r3.ID into R3
                         from r8 in R3.DefaultIfEmpty()
                         join r4 in db.Affixes on r1.PrefixId equals r4.Id into R4
                         from r5 in R4.DefaultIfEmpty()
                         join r6 in db.Affixes on r1.SuffixId equals r6.Id into R6
                         from r7 in R6.DefaultIfEmpty()
                         where r1.CharacterId == character.Id && r1.ItemId == r2.Id
                         select new ItemViewModel
                         {
                             GeneratedItem = r1,
                             Item = r2,
                             Image = r8 != null ? r8 : null,
                             Prefix = r5 != null ? r5 : null,
                             Suffix = r7 != null ? r7 : null
                         };

            List<ItemViewModel> characterItems = query2.ToList();

            EquipmentViewModel equipmentViewModel = new EquipmentViewModel();

            equipmentViewModel.EquippedItems =
                characterItems.FindAll(i => i.GeneratedItem.Status == ItemStatus.Equipped);
            equipmentViewModel.BackpackItems =
                characterItems.FindAll(i => i.GeneratedItem.Status == ItemStatus.Bagpack);
            equipmentViewModel.ChestItems =
                characterItems.FindAll(i => i.GeneratedItem.Status == ItemStatus.Chest);

            return View(equipmentViewModel);
        }
        
        public ActionResult ChangeStatusToEquipped(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Character character = this.HttpContext.Items["Character"] as Character;

            if (character == null)
            {
                return HttpNotFound();
            }

            var query = from r1 in db.GeneratedItems
                        from r2 in db.Items
                        where r1.CharacterId == character.Id && r1.ItemId == r2.Id && r1.Id == id && r1.Status == ItemStatus.Chest
                        select new ItemViewModel
                        {
                            GeneratedItem = r1,
                            Item = r2,
                        };

            // przedmiot do założenia
            ItemViewModel itemToEquip = query.FirstOrDefault();

            if (itemToEquip == null)
            {
                return RedirectToAction("Index");
            }

            var query2 = from r1 in db.GeneratedItems
                         from r2 in db.Items
                         where r1.CharacterId == character.Id && r1.ItemId == r2.Id && r1.Status == ItemStatus.Equipped
                         select new ItemViewModel
                         {
                             GeneratedItem = r1,
                             Item = r2,
                         };

            // lista przedmiotów postaci
            List<ItemViewModel> equippedItems = query2.ToList();

            CharacterViewModel characterViewModel = cs.GetCharacterViewModel(character, equippedItems);

            if (characterViewModel.Strength < itemToEquip.Item.RequireStrength)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Info, "Nie możesz założyć tego przedmiotu, masz za mało siły.");
                return RedirectToAction("Index");
            }

            if (itemToEquip.Item.Type == ItemType.Armor)
            {
                if (equippedItems.Count(i => i.Item.Type == ItemType.Armor) == 0)
                {
                    itemToEquip.GeneratedItem.Status = ItemStatus.Equipped;
                }
            }
            else if (itemToEquip.Item.Type == ItemType.Jewelry)
            {
                if (itemToEquip.Item.SubType == SubType.Amulet)
                {
                    if (equippedItems.Count(i => i.Item.SubType == SubType.Amulet) == 0)
                    {
                        itemToEquip.GeneratedItem.Status = ItemStatus.Equipped;
                    }
                }
                else
                {
                    if (equippedItems.Count(i => i.Item.SubType == SubType.Ring) < 2)
                    {
                        itemToEquip.GeneratedItem.Status = ItemStatus.Equipped;
                    }
                }
            }
            else if (itemToEquip.Item.Type == ItemType.Weapon)
            {
                if (equippedItems.Count(i => i.Item.Type == ItemType.Weapon) == 0)
                {
                    if (equippedItems.Count(i => i.Item.Type == ItemType.Shield) == 0)
                    {
                        itemToEquip.GeneratedItem.Status = ItemStatus.Equipped;
                    }
                }
            }
            else if (equippedItems.Count(i => i.Item.Type == ItemType.Shield) == 0)
            {
                if (equippedItems.Count(i => i.Item.SubType == SubType.TwoHanded) == 0)
                {
                    itemToEquip.GeneratedItem.Status = ItemStatus.Equipped;
                }
            }

            if (itemToEquip.GeneratedItem.Status != ItemStatus.Equipped)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Info, "Nie masz wolnego miejsca by założyć ten przedmiot.");
                return RedirectToAction("Index");
            }

            try
            {
                db.Entry(itemToEquip.GeneratedItem).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Wystąpił nieoczekiwany błąd. Skontaktuj się z administratorem.");
            }

            return RedirectToAction("Index");
        }

        public ActionResult ChangeStatusToChest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Character character = this.HttpContext.Items["Character"] as Character;

            if (character == null)
            {
                return HttpNotFound();
            }

            var query = from i in db.GeneratedItems
                        where i.CharacterId == character.Id
                        select i;

            List<GeneratedItem> items = query.ToList();

            var item = items.FirstOrDefault(i => i.Id == id && i.Status != ItemStatus.Chest);

            if (item == null)
            {
                return RedirectToAction("Index");
            }

            if (items.Count(i => i.Status == ItemStatus.Chest) >= 10)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Info, "Twoja skrzynia jest już pełna");
                return RedirectToAction("Index");
            }

            try
            {
                item.Status = ItemStatus.Chest;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Wystąpił nieoczekiwany błąd. Skontaktuj się z administratorem.");
            }

            return RedirectToAction("Index");
        }
	}
}
