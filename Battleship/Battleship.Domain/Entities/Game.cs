namespace Battleship.Domain.Entities
{
    public class Game
    {
        public string Id { get; } = Guid.NewGuid().ToString(); // Unique identifier for the game
        public const int GridSize = 10;
        public readonly char[,] grid = new char[GridSize, GridSize];
        public readonly List<Ship> ships = new List<Ship>();

        public Game(string id)
        {
            Id = id;
            grid = new char[GridSize, GridSize];
            ships = new List<Ship>();
        }

        public Game()
        {
            // Initialize grid
            for (int i = 0; i < GridSize; i++)
                for (int j = 0; j < GridSize; j++)
                    grid[i, j] = 'O'; // 'O' represents water

            // Place ships randomly
            ships.Add(new Ship(5, 'B')); // 1x Battleship (5 squares)
            ships.Add(new Ship(4, 'D')); // 2x Destroyers (4 squares)
            ships.Add(new Ship(4, 'D'));

            foreach (var ship in ships)
            {
                PlaceShipRandomly(ship);
            }
        }

        private void PlaceShipRandomly(Ship ship)
        {
            Random rand = new Random();
            bool placed = false;

            while (!placed)
            {
                bool isVertical = rand.Next(0, 2) == 0;
                int x = rand.Next(0, GridSize - (isVertical ? ship.Size : 0));
                int y = rand.Next(0, GridSize - (!isVertical ? ship.Size : 0));

                if (CanPlaceShip(x, y, ship.Size, isVertical))
                {
                    for (int i = 0; i < ship.Size; i++)
                    {
                        if (isVertical)
                            grid[x + i, y] = ship.Symbol;
                        else
                            grid[x, y + i] = ship.Symbol;
                    }
                    placed = true;
                }
            }
        }

        private bool CanPlaceShip(int x, int y, int size, bool isVertical)
        {
            for (int i = 0; i < size; i++)
            {
                if (grid[isVertical ? x + i : x, isVertical ? y : y + i] != 'O')
                    return false;
            }
            return true;
        }
    }
}