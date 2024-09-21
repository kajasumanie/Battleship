using Battleship.Application.Services;
using Battleship.Domain.Entities;
using Battleship.Infrastructure.Repository.Interfaces;
using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Application.Tests.Services
{
    public class GameServiceTests
    {
        private readonly GameService _gameService;
        private readonly Mock<IGameRepository> _mockGameRepository;

        public GameServiceTests()
        {
            _mockGameRepository = new Mock<IGameRepository>();
            var game = new Game(); // Use the default constructor to initialize
            _gameService = new GameService(game, _mockGameRepository.Object);
        }

        [Fact]
        public void InitializeGame_ShouldPlaceShipsAndSaveGameState()
        {
            // Act
            var gameId = _gameService.InitializeGame();

            // Assert
            _mockGameRepository.Verify(repo => repo.Save(It.IsAny<string>(), It.IsAny<Game>()), Times.Once);
            Assert.NotNull(gameId);
        }

        [Fact]
        public void Shoot_ShouldReturnMiss_WhenShootingAtEmptyCell()
        {
            // Arrange
            var gameId = "test-game-id";
            var game = new Game();
            _mockGameRepository.Setup(repo => repo.GetGame(gameId)).Returns(game);

            // Act
            var result = _gameService.Shoot(0, 0, gameId);

            // Assert
            Assert.Equal("Miss!", result);
            Assert.Equal('M', game.grid[0, 0]); // Ensure the cell is marked as miss
            _mockGameRepository.Verify(repo => repo.Save(gameId, game), Times.Once);
        }

        [Fact]
        public void Shoot_ShouldReturnHitAndSinkShip_WhenShootingAtShipAndSinksIt()
        {
            // Arrange
            var gameId = "test-game-id";
            var game = new Game();
            game.ships[0].Hits = 4; // Set hits to size - 1
            game.grid[0, 0] = game.ships[0].Symbol; // Place ship at (0,0)

            _mockGameRepository.Setup(repo => repo.GetGame(gameId)).Returns(game);

            // Act
            var result = _gameService.Shoot(0, 0, gameId);

            // Assert
            Assert.Equal($"Hit! You sunk a {game.ships[0].Name}!", result);
            Assert.Equal('X', game.grid[0, 0]); // Ensure the cell is marked as hit
            _mockGameRepository.Verify(repo => repo.Save(gameId, game), Times.Exactly(2)); // Save twice: once for hit, once for sink
        }

        [Fact]
        public void GetStatus_ShouldThrowException_WhenGameNotFound()
        {
            // Arrange
            var gameId = "non-existent-game-id";
            _mockGameRepository.Setup(repo => repo.GetGame(gameId)).Returns((Game)null);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _gameService.GetStatus(gameId));
            Assert.Equal("Game not found.", exception.Message);
        }

        [Fact]
        public void GetStatus_ShouldReturnGameGridAsStrings()
        {
            // Arrange
            var gameId = "test-game-id";
            var game = new Game();
            _mockGameRepository.Setup(repo => repo.GetGame(gameId)).Returns(game);

            // Act
            var result = _gameService.GetStatus(gameId);

            // Assert
            Assert.Equal(Game.GridSize, result.Count);
            for (int i = 0; i < Game.GridSize; i++)
            {
                Assert.Equal(new string('O', Game.GridSize), result[i]); // Each row should be filled with 'O'
            }
        }
    }
}