using Battleship.Application.Interfaces;
using Battleship.Domain.Entities;
using Battleship.Infrastructure.Repository.Interfaces;

namespace Battleship.Application.Services
{
    public class GameService : IGameService
    {
        private readonly Game _game;
        private readonly IGameRepository _gameRepository;

        public GameService(Game game, IGameRepository gameRepository)
        {
            _game = game;
            _gameRepository = gameRepository;
        }

        // Initialize the game, possibly by placing ships randomly
        public string InitializeGame()
        {
            foreach (var ship in _game.ships)
            {
                PlaceShipRandomly(ship);
            }
            SaveGameState(_game);  // Save the initialized game state
            return _game.Id;
        }

        // Places a ship randomly on the grid
        private void PlaceShipRandomly(Ship ship)
        {
            Random rand = new Random();
            bool placed = false;

            while (!placed)
            {
                bool isVertical = rand.Next(0, 2) == 0;
                int x = rand.Next(0, Game.GridSize - (isVertical ? ship.Size : 0));
                int y = rand.Next(0, Game.GridSize - (!isVertical ? ship.Size : 0));

                if (CanPlaceShip(x, y, ship.Size, isVertical))
                {
                    for (int i = 0; i < ship.Size; i++)
                    {
                        if (isVertical)
                            _game.grid[x + i, y] = ship.Symbol;
                        else
                            _game.grid[x, y + i] = ship.Symbol;
                    }
                    placed = true;
                }
            }

            SaveGameState(_game);  // Save the updated game state after placing a ship
        }

        // Check if a ship can be placed at the specified location
        private bool CanPlaceShip(int x, int y, int size, bool isVertical)
        {
            for (int i = 0; i < size; i++)
            {
                if (_game.grid[isVertical ? x + i : x, isVertical ? y : y + i] != 'O')
                    return false;
            }
            return true;
        }

        // Process a shot at the given coordinates (x, y)
        public string Shoot(int x, int y,  string gameId)
        {
            var game = _gameRepository.GetGame(gameId);
            if (game.grid[x, y] == 'O')
            {
                game.grid[x, y] = 'M'; // Miss
                SaveGameState(_game);  // Save updated game state
                return "Miss!";
            }

            char shipSymbol = game.grid[x, y];
            game.grid[x, y] = 'X'; // Hit

            var ship = game.ships.First(s => s.Symbol == shipSymbol);
            ship.Hits++;

            if (ship.Hits == ship.Size)
            {
                SaveGameState(game);  // Save updated game state after sinking a ship
                return $"Hit! You sunk a {ship.Name}!";
            }

            SaveGameState(_game);  // Save updated game state after a hit
            return "Hit!";
        }

        public List<string> GetStatus(string gameId)
        {
            var game = _gameRepository.GetGame(gameId); // Retrieve game from repository

            if (game == null)
            {
                throw new Exception("Game not found."); // Handle game not found
            }

            var result = new List<string>();

            for (int i = 0; i < Game.GridSize; i++)
            {
                var row = new char[Game.GridSize];
                for (int j = 0; j < Game.GridSize; j++)
                {
                    row[j] = game.grid[i, j]; // Use the retrieved game grid
                }
                result.Add(new string(row));
            }

            return result;
        }
        // Save the current game state to the repository
        private void SaveGameState(Game game)
        {
            _gameRepository.Save(game.Id, game); // Persist the game state with a unique game ID
        }
    }
}
