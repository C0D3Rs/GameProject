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
            if (!context.Items.Contains("NotCompletedEventLog"))
            {
                return 0;
            }

            EventLog eventTime = context.Items["NotCompletedEventLog"] as EventLog;

            DateTime maxTime = DateTime.Now;
            maxTime.AddSeconds(-10);

            return (int)(eventTime.Created_at - maxTime).TotalMilliseconds;
        }
    }
}
