namespace HigherOrLower.Api.Controllers
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using HigherOrLower.Api.Mapper;
	using HigherOrLower.Contracts.Parameters;
	using HigherOrLower.Contracts.Results;
	using HigherOrLower.Domain.Card;
	using HigherOrLower.Domain.Game;
	using HigherOrLower.Infrastructure.Repositories;
	using Microsoft.AspNetCore.Mvc;

	[Route("api/[controller]")]
	[ApiController]
	public class GamesController : ControllerBase
	{
		private readonly IGameRepository _gameRepository;

		public GamesController(IGameRepository gameRepository)
		{
			_gameRepository = gameRepository
				?? throw new ArgumentNullException(nameof(gameRepository));
		}

		//POST api/games
		[HttpPost]
		[ProducesResponseType(typeof(GetGameResults), 200)]
		public async Task<IActionResult> CreateGame(CreateGameParameters parameters)
		{
			var gameId = Guid.NewGuid();

			var card = Card.CreateNew(
				Guid.NewGuid(),
				gameId);

			var newGame = Game.CreateNew(
				id: Guid.NewGuid(),
				name: parameters.Name,
				card: card,
				numberOfCards: parameters.NumberOfCards.GetValueOrDefault());

			GetGameResults result = null;

			await _gameRepository.CreateGameAsync(newGame);

			if (await _gameRepository.CompleteAsync() > 0)
			{
				result = newGame.MapToContract();
			}
			else
			{
				return BadRequest();
			}

			return Ok(result);
		}

		//GET api/games
		[HttpGet]
		[ProducesResponseType(typeof(GetGamesResults), 200)]
		public async Task<IActionResult> GetAllGamesAsync(
			CancellationToken ct = default)
		{
			var games = await _gameRepository.GetAllAvailableGamesAsync(ct);

			if (games == null || !games.Any())
			{
				return NotFound();
			}

			var result = new GetGamesResults(
				games.Select(a =>
				a.MapToContract()));

			return Ok(result);
		}

		//GET api/games/{id}
		[HttpGet("{id:Guid}")]
		[ProducesResponseType(typeof(GetGameResults), 200)]
		public async Task<IActionResult> GetGameByIdAsync(
			Guid id,
			CancellationToken ct = default)
		{
			var game = await _gameRepository.GetGameByIdAsync(id, ct);

			if (game == null)
			{
				return NotFound();
			}

			return Ok(game.MapToContract());
		}

		//GET api/games/play
		[HttpPost("play")]
		public async Task<IActionResult> LetsPlayGameAsync(
			PlayGame playGame,
			CancellationToken ct = default)
		{
			var game = await _gameRepository
				.GetGameByIdAsync(playGame.GameId, ct);

			if (game == null)
			{
				return NotFound();
			}
			else if(game.IsGameOver)
			{
				return Ok(new GamePlayResults(
					gameId: playGame.GameId,
					message: "The Game is Over!",
					number: game.Card.FaceValue,
					isWinner: null));
			}

			GamePlayResults gamePlayResults;

			if ((playGame.IsNextNumberHigher && game.Card.NextValue >= game.Card.FaceValue) ||
				(!playGame.IsNextNumberHigher && game.Card.NextValue < game.Card.FaceValue)) 
			{
				gamePlayResults = new GamePlayResults(
					gameId: playGame.GameId,
					message: "You Won! Congratulations!",
					number: game.Card.NextValue,
					isWinner: true);
			}
			else
			{
				gamePlayResults = new GamePlayResults(
					gameId: playGame.GameId, 
					message: "You lost! Better luck next time!", 
					number: game.Card.NextValue,
					isWinner: false);
			}

			return Ok(gamePlayResults);
		}

		//PUT api/games/{id}
		[HttpPut("nextplayer")]
		public async Task<IActionResult> UpdateGame(
			Guid gameId,
			CancellationToken ct = default)
		{
			var game = await _gameRepository.GetGameByIdAsync(gameId, ct);

			if (game == null)
			{
				return NotFound();
			}

			game.PrepareForNextPlayer();
			game.Card.SetANewNumber();

			_gameRepository.UpdateGame(game);
			await _gameRepository.CompleteAsync();

			return Ok();
		}
	}
}
