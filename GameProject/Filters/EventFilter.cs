using GameProject.Enums;
using GameProject.Models;
using GameProject.Models.Entities;
using GameProject.Services;
using GameProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GameProject.Filters
{
    public class EventFilter : ActionFilterAttribute, IActionFilter, IDisposable
    {
        private DatabaseContext db = new DatabaseContext();

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(!filterContext.HttpContext.Items.Contains("Character"))
            {
                this.OnActionExecuting(filterContext);
                return;
            }

            Character character = filterContext.HttpContext.Items["Character"] as Character;

            var query = from el in db.EventLogs.AsNoTracking()
                        where el.CharacterId == character.Id && el.IsCompleted == false
                        orderby el.Created_at descending
                        select el;

            var eventLog = query.FirstOrDefault();

            if (eventLog == null)
            {
                return;
            }

            DateTime timeNow = DateTime.Now;

            double differentSeconds = (timeNow - eventLog.Created_at).TotalSeconds;

            if (differentSeconds < 10)
            {
                filterContext.HttpContext.Items.Add("NotCompletedEventLog", eventLog);
                this.OnActionExecuting(filterContext);
                return;
            }

            var query2 = from e in db.Events.AsNoTracking()
                         where e.Id == eventLog.EventId
                         select e;

            var oneEvent = query2.FirstOrDefault();

            if(oneEvent == null)
            {
                this.OnActionExecuting(filterContext);
                return;
            }

            EventService es = new EventService();
            CharacterService cs = new CharacterService();
            ItemService itemService = new ItemService();
            Random dice = new Random();

            string messageContent = es.GetMainDescription(oneEvent);
            bool characterWinner = false;

            if (oneEvent.Type == EventType.Monster)
            {
                var query3 = from m in db.Monsters.AsNoTracking()
                             where m.Id == oneEvent.MonsterId
                             select m;

                var monster = query3.FirstOrDefault();

                if (monster == null)
                {
                    this.OnActionExecuting(filterContext);
                    return;
                }

                var query4 = from gi in db.GeneratedItems
                             from i in db.Items
                             where gi.CharacterId == character.Id &&
                                   gi.ItemId == i.Id &&
                                   gi.Status == ItemStatus.Equipped
                             select new ItemViewModel
                             {
                                 GeneratedItem = gi,
                                 Item = i
                             };

                List<ItemViewModel> items = query4.ToList();

                CharacterViewModel characterViewModel = cs.GetCharacterViewModel(character, items.FindAll(i => i.GeneratedItem.Durability > 0));

                messageContent += es.GetBattleReport(characterViewModel, monster, ref characterWinner);

                if(characterWinner)
                {
                    var query5 = from i in db.GeneratedItems
                                where i.CharacterId == character.Id && i.Status == ItemStatus.Bagpack
                                select i;

                    if (query5.Count() == 0)
                    {
                        var item = itemService.GetRandomItem(monster.Level);

                        if (item != null)
                        {
                            int random = dice.Next(0, 100);

                            Affix prefix = null;

                            if (random >= 50)
                            {
                                prefix = itemService.GetRandomAffix(AffixType.Prefix, item.Type, item.QualityLevel);
                            }

                            random = dice.Next(0, 100);

                            Affix suffix = null;

                            if (random >= 75)
                            {
                                suffix = itemService.GetRandomAffix(AffixType.Suffix, item.Type, item.QualityLevel);
                            }

                            GeneratedItem generatedItem = itemService.GetGeneratedItem(item, prefix, suffix);

                            if (generatedItem == null)
                            {
                                return;
                            }

                            generatedItem.Status = ItemStatus.Bagpack;
                            generatedItem.CharacterId = character.Id;

                            db.GeneratedItems.Add(generatedItem);
                        }
                    }

                    foreach (var item in items)
                    {
                        item.GeneratedItem.Durability -= 5;
                        item.GeneratedItem.Durability = item.GeneratedItem.Durability < 0 ? 0 : item.GeneratedItem.Durability;
                        db.Entry(item.GeneratedItem).State = EntityState.Modified;
                    }
                }
                else
                {
                    foreach (var item in items)
                    {
                        item.GeneratedItem.Durability -= item.Item.Durability / 3;
                        item.GeneratedItem.Durability = item.GeneratedItem.Durability < 0 ? 0 : item.GeneratedItem.Durability;
                        db.Entry(item.GeneratedItem).State = EntityState.Modified;
                    }
                }
            }
            else
            {
                var query3 = from gi in db.GeneratedItems
                             from i in db.Items
                             where gi.CharacterId == character.Id &&
                                   gi.ItemId == i.Id &&
                                   gi.Status == ItemStatus.Equipped
                             select new ItemViewModel
                             {
                                 GeneratedItem = gi,
                                 Item = i
                             };

                List<ItemViewModel> items = query3.ToList();
                CharacterViewModel characterViewModel = cs.GetCharacterViewModel(character, items);

                characterWinner = es.CheckCharacterMeetsEventRequiment(oneEvent, characterViewModel);

                if (characterWinner)
                {
                    character.Gold += oneEvent.Reward;
                }
            }

            messageContent += es.GetResultDescription(oneEvent, characterWinner);

            Message message = new Message()
            {
                Type = MessageType.System,
                Title = "Raport ze zdarzenia",
                ContentOfMessage = messageContent,
                Date = DateTime.Now,
                ToUserId = character.Id,
                FromUser = "System"
            };

            eventLog.IsCompleted = true;
            
            try
            {
                if (characterWinner && oneEvent.Type == EventType.Random)
                {
                    db.Entry(character).State = EntityState.Modified;
                }

                db.Entry(eventLog).State = EntityState.Modified;

                db.Messages.Add(message);
                db.SaveChanges();
            }
            catch(Exception)
            {
                // ??
            }

            if (characterWinner && oneEvent.Type == EventType.Random)
            {
                filterContext.HttpContext.Items["Character"] = character;
            }
            
            this.OnActionExecuting(filterContext);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
