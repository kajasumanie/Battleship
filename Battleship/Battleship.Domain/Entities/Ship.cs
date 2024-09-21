using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Domain.Entities
{
    public class Ship
    {
        public int Size { get; }
        public char Symbol { get; }
        public int Hits { get; set; } = 0;
        public string Name => Symbol == 'B' ? "Battleship" : "Destroyer";

        public Ship(int size, char symbol)
        {
            Size = size;
            Symbol = symbol;
        }
    }
}