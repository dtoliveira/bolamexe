using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TopSunday.Models;

namespace TopSunday
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("name=ApplicationDbContext")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<GameDay> GameDay { get; set; }
        public virtual DbSet<Classification> Classification { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<MVP> MVP { get; set; }
        public virtual DbSet<GoldBidon> GoldBidon { get; set; }

    }
}