using Battleship.Domain.Entities;
using Battleship.Infrastructure.Repository;

namespace Battleship.Infrastructure.Tests.Repository
{
    public class GameRepositoryTests
    {
        private readonly GameRepository _gameRepository;

        public GameRepositoryTests()
        {
            _gameRepository = new GameRepository();
        }

        [Fact]
        public void Save_ShouldAddGameToStorage_WhenNewGameIsProvided()
        {
            // Arrange
            var gameId = "test-game-id";
            var game = new Game(); // Use the default constructor

            // Act
            _gameRepository.Save(gameId, game);

            // Assert
            var retrievedGame = _gameRepository.GetGame(gameId);
            Assert.NotNull(retrievedGame);
            Assert.Equal(game.Id, retrievedGame.Id); // Ensure IDs match
        }

        [Fact]
        public void Save_ShouldUpdateExistingGame_WhenSameGameIdIsProvided()
        {
            // Arrange
            var gameId = "test-game-id";
            var initialGame = new Game();
            _gameRepository.Save(gameId, initialGame);

            // Act
            var updatedGame = new Game(); // Use the default constructor
            updatedGame.ships.Add(new Ship(2, 'D')); // Example update
            _gameRepository.Save(gameId, updatedGame);

            // Assert
            var retrievedGame = _gameRepository.GetGame(gameId);
            Assert.NotNull(retrievedGame);
            Assert.Equal(updatedGame.Id, retrievedGame.Id); // Ensure IDs match
        }

        [Fact]
        public void GetGame_ShouldReturnNull_WhenGameIdDoesNotExist()
        {
            // Arrange
            var gameId = "non-existent-game-id";

            // Act
            var result = _gameRepository.GetGame(gameId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Delete_ShouldRemoveGame_WhenGameIdExists()
        {
            // Arrange
            var gameId = "test-game-id";
            var game = new Game();
            _gameRepository.Save(gameId, game);

            // Act
            _gameRepository.Delete(gameId);

            // Assert
            var result = _gameRepository.GetGame(gameId);
            Assert.Null(result);
        }

        [Fact]
        public void Delete_ShouldDoNothing_WhenGameIdDoesNotExist()
        {
            // Arrange
            var gameId = "non-existent-game-id";

            // Act
            _gameRepository.Delete(gameId);

            // Assert
            var result = _gameRepository.GetGame(gameId);
            Assert.Null(result);
        }
    }
}