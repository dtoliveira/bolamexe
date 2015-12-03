using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TopSunday.Models;
using TopSunday.Services.Interfaces;

namespace TopSunday.Services.Repositories
{
    public class PlayerRepository : IPlayerRepository, IRepositoryBase
    {
        private ApplicationDbContext context;

        public PlayerRepository(ApplicationDbContext Context)
        {
            context = Context;
            //context.Configuration.ProxyCreationEnabled = false;
        }
        public IQueryable<Player> All
        {
            get
            {
                return context.Player;
            }
        }


        public Player Find(int id)
        {
            return context.Player.Find(id);
        }

        public void InsertOrUpdate(Player Player)
        {
            if (Player.ID == default(int))
            {
                context.Player.Add(Player);
            }
            else
            {
                context.Entry(Player).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var Player = context.Player.Find(id);
            context.Player.Remove(Player);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}