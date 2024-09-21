using Battleship.Application.DTOs;
using Battleship.Application.Interfaces;
using Battleship.Domain.Entities;

using Microsoft.AspNetCore.Mvc;

namespace Battleship.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BattleshipController : ControllerBase
    {
        private static Game game = new Game();
        private readonly IGameService _gameService;

        public BattleshipController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("shoot/{gameId}")]
        public IActionResult Shoot([FromBody] ShotRequest request, string gameId)
        {
            var result = _gameService.Shoot(request.X, request.Y, gameId);
            return Ok(result);
        }

        [HttpGet("status/{gameId}")]
        public IActionResult GetStatus(string gameId)
        {
            var gridStatus = _gameService.GetStatus(gameId);
            return Ok(gridStatus);
        }

        [HttpPost("reset")]
        public IActionResult ResetGame()
        {
            game = new Game();  // Create a new game instance
            return Ok("Game reset!");
        }

        [HttpPost("initialize")]
        public IActionResult InitializeGame()
        {
            var cc  =_gameService.InitializeGame(); // Make sure this properly initializes the game
            return Ok(cc); // Return the grid status
        }

    }
}
