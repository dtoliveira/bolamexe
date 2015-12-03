using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TopSunday.Models;

namespace TopSunday.Services.Interfaces
{
    interface IPlayerRepository : IDisposable
    {
        IQueryable<Player> All { get; }

        Player Find(int id);

        void InsertOrUpdate(Player Player);

        void Delete(int id);

        void Save();
    }
}
