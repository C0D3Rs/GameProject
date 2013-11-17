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

namespace GameProject.Controllers
{
    [AuthorizationFilter(UserRole.Normal)]
    public class EquipmentController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private UserSessionContext us = new UserSessionContext();
        private ItemService itemService = new ItemService();

        public ActionResult Index()
        {
            us.SetHttpSessionStateBase(this.HttpContext.Session);
            var user = us.GetUser();

            if (user == null)
            {
                HttpNotFound();
            }

            var query = from i in db.GeneratedItems
                        where i.UserId == user.Id
                        select i;

            List<GeneratedItem> UserItems = query.ToList();

            EquipmentViewModel equipmentViewModel = new EquipmentViewModel();

            equipmentViewModel.EquippedItems = 
                UserItems.FindAll(i => i.Status == ItemStatus.Equipped);
            equipmentViewModel.BackpackItems = 
                UserItems.FindAll(i => i.Status == ItemStatus.Bagpack);
            equipmentViewModel.ChestItems = 
                UserItems.FindAll(i => i.Status == ItemStatus.Chest);

            return View(equipmentViewModel);
        }

        public ActionResult ChangeStatusToEquipped(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            us.SetHttpSessionStateBase(this.HttpContext.Session);
            int UserId = us.GetUserId();

            var query = from i in db.GeneratedItems
                        where i.UserId == UserId
                        select i;

            List<GeneratedItem> userItems = query.ToList();

            var item = userItems.FirstOrDefault(i => i.Id == id && i.Status == ItemStatus.Chest);

            if (item == null)
            {
                return RedirectToAction("Index");
            }

            List<GeneratedItem> equippedItems = userItems.FindAll(i => i.Status == ItemStatus.Equipped);

            if (item.Type == ItemType.Armor)
            {
                if (equippedItems.Count(i => i.Type == ItemType.Armor) == 0)
                {
                    item.Status = ItemStatus.Equipped;
                }
            }
            else if (item.Type == ItemType.Jewelry)
            {
                if (item.SubType == SubType.Amulet)
                {
                    if (equippedItems.Count(i => i.SubType == SubType.Amulet) == 0)
                    {
                        item.Status = ItemStatus.Equipped;
                    }
                }
                else
                {
                    if (equippedItems.Count(i => i.SubType == SubType.Ring) < 2)
                    {
                        item.Status = ItemStatus.Equipped;
                    }
                }
            }
            else if (item.Type == ItemType.Weapon)
            {
                if (equippedItems.Count(i => i.Type == ItemType.Weapon) == 0)
                {
                    if (equippedItems.Count(i => i.Type == ItemType.Shield) == 0)
                    {
                        item.Status = ItemStatus.Equipped;
                    }
                }
            }
            else if (equippedItems.Count(i => i.Type == ItemType.Shield) == 0)
            {
                if (equippedItems.Count(i => i.SubType == SubType.TwoHanded) == 0)
                {
                    item.Status = ItemStatus.Equipped;
                }
            }

            if (item.Status != ItemStatus.Equipped)
            {
                return RedirectToAction("Index");
            }

            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception)
            {
                // komunikat jakiś
            }

            return RedirectToAction("Index");
        }

        public ActionResult ChangeStatusToChest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            us.SetHttpSessionStateBase(this.HttpContext.Session);
            int UserId = us.GetUserId();

            var query = from i in db.GeneratedItems
                        where i.UserId == UserId
                        select i;

            List<GeneratedItem> items = query.ToList();

            var item = items.FirstOrDefault(i => i.Id == id && i.Status != ItemStatus.Chest);

            if (item == null)
            {
                return RedirectToAction("Index");
            }

            if (items.Count(i => i.Status == ItemStatus.Chest) >= 10)
            {
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
                // komunikat jakiś
            }

            return RedirectToAction("Index");
        }

        public ActionResult GenerateItem(int qualityLevel = 0)
        {
            if (qualityLevel <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            us.SetHttpSessionStateBase(this.HttpContext.Session);
            int UserId = us.GetUserId();

            var query = from i in db.GeneratedItems
                        where i.UserId == UserId && i.Status == ItemStatus.Bagpack
                        select i;

            if (query.Count() != 0)
            {
                return RedirectToAction("Index");
            }

            var item = itemService.GetRandomItem(qualityLevel);

            if (item == null)
            {
                return RedirectToAction("Index");
            }

            var prefix = itemService.GetRandomAffix(AffixType.Prefix, item.Type, qualityLevel);
            var suffix = itemService.GetRandomAffix(AffixType.Suffix, item.Type, qualityLevel);

            var generatedItem = itemService.GetGeneratedItem(item, prefix, suffix);

            if (generatedItem == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                generatedItem.Status = ItemStatus.Bagpack;
                generatedItem.UserId = UserId;
                db.GeneratedItems.Add(generatedItem);
                db.SaveChanges();
            }
            catch (Exception)
            {
                // komunikat jakiś
            }

            return RedirectToAction("Index");
        }
	}
}