using GameProject.Models.Entities;
using GameProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}
