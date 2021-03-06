﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameProject.Models.Entities;
using GameProject.Models;

using GameProject.Enums;
using GameProject.Filters;
using GameProject.Helpers;
using GameProject.Services;
using GameProject.ViewModels;



namespace GameProject.Controllers
{
    [AuthorizationFilter(UserRole.Normal, Order = 1)]
    [CharacterCreatorFilter(Order = 2)]
    [EventFilter(Order = 3)]
    [CharacterResourcesFilter(Order = 4)]
    public class MessageController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private CharacterService cs = new CharacterService();

        // GET: /Message/
        public ActionResult Index()
        {
            Character character = this.HttpContext.Items["Character"] as Character;

            if (character == null)
            {
                return HttpNotFound();
            }

            var query_message = from d1 in db.Messages
                                where d1.ToUserId == character.Id
                                orderby d1.Date
                                select d1;

            var messages = query_message.Take(10).ToList();
            int count_messages = query_message.Count();
            ViewData["count_messages"] = count_messages;
            messages.Reverse();
            return View(messages);
        }

        // GET: /Message/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            MessageService ms_service = new MessageService();
            if (message.Type == MessageType.System)
            {
                message.ContentOfMessage = ms_service.GetHtmlSystemRaport(message.ContentOfMessage);
            }
            return View(message);
        }

        // GET: /Message/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Message/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(Message message, string ToUserName)
        {
            Character character = this.HttpContext.Items["Character"] as Character;
            message.FromUser = character.Name;

            var query = from d1 in db.Characters
                        where d1.Name == ToUserName
                        select d1.Id;

            if (ToUserName == character.Name)
            {
                var oneCharacter = query.FirstOrDefault();
                message.ToUserId = oneCharacter;
                message.Type = MessageType.User;
            }
            else
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Warning, "Nazwa użytkownika jest nie poprawna lub nie istnieje w bazie danych.");
                return View("Create");
            }

            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(message);
        }

        // GET: /Message/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(IEnumerable<int> messages_checkbox)
        {
            if (messages_checkbox == null)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Warning, "Nie zaznaczono wiadomości");
                return RedirectToAction("Index");
            }

            db.Messages.Where(x => messages_checkbox.Contains(x.Id)).ToList().ForEach(x => db.Messages.Remove(x));
            db.SaveChanges();
            return RedirectToAction("Index");
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
