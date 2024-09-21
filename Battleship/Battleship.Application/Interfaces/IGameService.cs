using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Application.Interfaces
{
    public interface IGameService
    {
        string InitializeGame();
        string Shoot(int x, int y, string gameId);
        List<string> GetStatus(string gameId);
    }
}