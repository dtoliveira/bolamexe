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

        public virtual DbSet<Classification> Classification { get; set; }
        public virtual DbSet<GameType> GameType { get; set; }
        public virtual DbSet<GameTeams> GameTeams { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<Season> Season { get; set; }
        public virtual DbSet<PlayerConfirmationGames> PlayerConfirmationGames { get; set; }
        public virtual DbSet<CurrentGame> CurrentGame { get; set; }
        public virtual DbSet<Players_GameType> Players_GameType { get; set; }
    }
}