using GameProject.Models;
using GameProject.Models.Entities;
using GameProject.Enums;
using GameProject.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace GameProject.Models
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            var users = new List<User>
            {
                new User { Name = "admin", Password = PasswordHashService.CreateHash("admin"), Role = UserRole.Admin },
            };

            users.ForEach(o => context.Users.Add(o));
            context.SaveChanges();
        }
    }
}
