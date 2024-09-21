using Battleship.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Infrastructure.Repository.Interfaces
{
    public interface IGameRepository
    {
        void Save(string gameId, Game game);
        Game GetGame(string gameId);
        void Delete(string gameId);
    }
}
