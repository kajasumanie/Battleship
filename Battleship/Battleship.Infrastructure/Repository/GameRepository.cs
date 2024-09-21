using Battleship.Domain.Entities;
using Battleship.Infrastructure.Repository.Interfaces;

namespace Battleship.Infrastructure.Repository
{
    public class GameRepository : IGameRepository
    {
        // This dictionary will store the game state in memory using a unique game ID.
        private readonly Dictionary<string, Game> _gameStorage;

        public GameRepository()
        {
            _gameStorage = new Dictionary<string, Game>();
        }

        // Save the game state with a unique ID.
        public void Save(string gameId, Game game)
        {
            if (_gameStorage.ContainsKey(gameId))
            {
                _gameStorage[gameId] = game; // Update the existing game state
            }
            else
            {
                _gameStorage.Add(gameId, game); // Add a new game state
            }
        }

        // Retrieve the game state by the unique game ID.
        public Game GetGame(string gameId)
        {
            if (_gameStorage.ContainsKey(gameId))
            {
                return _gameStorage[gameId];
            }

            // Return null or throw an exception if the game ID does not exist.
            return null;
        }

        // Optional: You can implement a method to delete a game from the storage if needed.
        public void Delete(string gameId)
        {
            if (_gameStorage.ContainsKey(gameId))
            {
                _gameStorage.Remove(gameId);
            }
        }
    }
}
