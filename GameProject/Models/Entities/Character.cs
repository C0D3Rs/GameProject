using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameProject.Models.Entities
{
    public class Character
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public CharacterClass Class { get; set; }

        public string Name { get; set; }

        public int Experience { get; set; }

        public int Gold { get; set; }
    }
}
