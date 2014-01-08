using GameProject.Models.Entities;
using GameProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameProject.Models;

namespace GameProject.Helpers
{
    public class CharacterHelpers
    {
        public static CharacterResourcesViewModel GetCharacterResources(HttpContextBase context)
        {
            if (!context.Items.Contains("CharacterResources"))
            {
                return null;
            }

            return context.Items["CharacterResources"] as CharacterResourcesViewModel;
        }

        public static int GetLeftTimeToEventComplete(HttpContextBase context)
        {
            if (!context.Items.Contains("Character"))
            {
                return 0;
            }

            var character = context.Items["Character"] as Character;

            using (DatabaseContext db = new DatabaseContext())
            {
                var eventlogs = from el in db.EventLogs.AsNoTracking()
                                where el.CharacterId == character.Id && el.IsCompleted == false
                                select el;

                var eventTime = eventlogs.FirstOrDefault();

                if (eventTime == null)
                {
                    return 0;
                }

                return (int)(DateTime.Now - eventTime.Created_at).TotalSeconds;
            }
                     
        }
    }
}
