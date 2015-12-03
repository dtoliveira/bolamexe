using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace League.Services.Interfaces
{
    interface IPlayerRepository: IDisposable
    {
        IQueryable<Player> All { get; }

        IQueryable<Player> AllIncluding(params Expression<Func<Player, object>>[] includeProperties);

        Player Find(int id);

        void InsertOrUpdate(Player Player);

        void Delete(int id);

        void Save();
    }
}
