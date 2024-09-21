using Battleship.Api.Controllers;
using Battleship.Application.DTOs;
using Battleship.Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Battleship.Api.Tests.Controllers
{
    public class BattleshipControllerTests
    {
        private readonly BattleshipController _controller;
        private readonly Mock<IGameService> _gameServiceMock;

        public BattleshipControllerTests()
        {
            _gameServiceMock = new Mock<IGameService>();  // Mock the game service
            _controller = new BattleshipController(_gameServiceMock.Object);  // Inject mock into controller
        }

        [Fact]
        public void Shoot_ShouldReturnOk_WithResult()
        {
            // Arrange
            var request = new ShotRequest { X = 1, Y = 1 };
            var gameId = "game123";
            var expectedResult = "Hit";
            _gameServiceMock
                .Setup(s => s.Shoot(request.X, request.Y, gameId))
                .Returns(expectedResult);  // Mock the Shoot method to return "Hit"

            // Act
            var result = _controller.Shoot(request, gameId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be(expectedResult);
        }

        [Fact]
        public void GetStatus_ShouldReturnOk_WithGridStatus()
        {
            // Arrange
            var gameId = "game123";
            var expectedGridStatus = new List<string>
            {
                "Row1", "Row2", "Row3", // Sample data; adjust based on your actual grid
            };
            _gameServiceMock
                .Setup(s => s.GetStatus(gameId))
                .Returns(expectedGridStatus);  // Mock GetStatus to return a List<string>

            // Act
            var result = _controller.GetStatus(gameId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();  // Assert the result is OkObjectResult
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be(expectedGridStatus);  // Assert the returned grid status
        }


        [Fact]
        public void ResetGame_ShouldReturnOk_WithResetMessage()
        {
            // Act
            var result = _controller.ResetGame();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be("Game reset!");
        }

        [Fact]
        public void Shoot_ShouldReturnOk_WithShotResult()
        {
            // Arrange
            var gameId = "game123";  // This should be a string representing the gameId
            var shotRequest = new ShotRequest { X = 5, Y = 3 };
            var expectedShotResult = "Hit!";  // Example response from shoot

            _gameServiceMock
                .Setup(s => s.Shoot(shotRequest.X, shotRequest.Y, gameId))
                .Returns(expectedShotResult);  // Mock the shoot method

            // Act
            var result = _controller.Shoot(shotRequest, gameId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();  // Ensure the result is OkObjectResult
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be(expectedShotResult);  // Ensure the correct shot result is returned
        }

    }
}