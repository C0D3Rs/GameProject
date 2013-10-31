using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace GameProject.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Shield> Shields { get; set; }
        public DbSet<Armor> Armors { get; set; }
        public DbSet<Jewelry> Jewelries { get; set; }

        public DbSet<Affix> Affixes { get; set; }

        public DatabaseContext()
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
