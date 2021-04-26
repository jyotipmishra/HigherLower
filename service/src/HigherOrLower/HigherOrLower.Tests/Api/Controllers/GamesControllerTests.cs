namespace HigherOrLower.Tests.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFixture;
    using FluentAssertions;
    using HigherOrLower.Api.Controllers;
    using HigherOrLower.Contracts.Parameters;
    using HigherOrLower.Contracts.Results;
    using HigherOrLower.Domain.Card;
    using HigherOrLower.Domain.Game;
    using HigherOrLower.Infrastructure.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
	public class GamesControllerTests
	{
        private static GamesController _gameController;
        private Mock<IGameRepository> _mockGameRepository;
        private Fixture _fixture;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockGameRepository = new Mock<IGameRepository>(MockBehavior.Strict);

            _fixture = new Fixture();

            _gameController
                = new GamesController(_mockGameRepository.Object);
        }

        [TestMethod]
        public async Task CreateGame_CreateGameSuccessfully()
		{
            //Arrange
            var parameters = _fixture.Create<CreateGameParameters>();
            var gameId = Guid.NewGuid();

            var card = Card.CreateNew(
                Guid.NewGuid(),
                gameId);

            var newGame = Game.CreateNew(
                id: Guid.NewGuid(),
                name: parameters.Name,
                card: card,
                numberOfCards: parameters.NumberOfCards.GetValueOrDefault());

            _mockGameRepository
                .Setup(exec => 
                    exec.CreateGameAsync(
                        It.IsAny<Game>()))
                .Returns(Task.CompletedTask);

            _mockGameRepository
                .Setup(exec =>
                    exec.CompleteAsync())
                .Returns(Task.FromResult(2));

            //Act
            var result = await _gameController
                .CreateGame(parameters) as OkObjectResult;

            //Assert
            _mockGameRepository
               .Verify(exec =>
               exec.CreateGameAsync(It.IsAny<Game>()),
               Times.Once);

            _mockGameRepository
                .Verify(exec =>
                exec.CompleteAsync(),
                Times.Once);

            result
                .Should()
                .NotBeNull();

            result.StatusCode
                .Should()
                .Be((int)HttpStatusCode.OK);

            var gameResult = result.Value as GetGameResults;

            gameResult.Name
               .Should()
               .Be(newGame.Name);
        }

        [TestMethod]
        public async Task CreateGame_CreateGameFailedWithBadRequest()
        {
            //Arrange
            var parameters = _fixture.Create<CreateGameParameters>();
            var gameId = Guid.NewGuid();

            var card = Card.CreateNew(
                Guid.NewGuid(),
                gameId);

            var newGame = Game.CreateNew(
                id: Guid.NewGuid(),
                name: parameters.Name,
                card: card,
                numberOfCards: parameters.NumberOfCards.GetValueOrDefault());

            _mockGameRepository
                .Setup(exec =>
                exec.CreateGameAsync(
                    It.IsAny<Game>()))
                .Returns(Task.CompletedTask);

            _mockGameRepository
                .Setup(exec =>
                exec.CompleteAsync())
                .Returns(Task.FromResult(-1));

            //Act
            var result = await _gameController
                .CreateGame(parameters) as BadRequestResult;

            //Assert
            _mockGameRepository
               .Verify(exec =>
               exec.CreateGameAsync(It.IsAny<Game>()),
               Times.Once);

            _mockGameRepository
                .Verify(exec =>
                exec.CompleteAsync(),
                Times.Once);

            result
                .Should()
                .NotBeNull();

            result.StatusCode
                .Should()
                .Be((int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task GetAllGames_ReturnsAllAvailableGames()
        {
            //Arrange
            var results = _fixture.Create<GetGamesResults>();

            var gamesInDb = _fixture.CreateMany<Game>(5);

            _mockGameRepository
                .Setup(exec =>
                    exec.GetAllAvailableGamesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(gamesInDb);

            //Act
            var result = await _gameController
                .GetAllGamesAsync() as OkObjectResult;

            //Assert
            _mockGameRepository
                .Verify(exec =>
                    exec.GetAllAvailableGamesAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once);

            result
                .Should()
                .NotBeNull();

            result.StatusCode
                .Should()
                .Be((int)HttpStatusCode.OK);

            var gamesResult = result.Value as GetGamesResults;

            gamesResult.Games.Count()
               .Should()
               .Be(gamesInDb.Count());
        }

        [TestMethod]
        public async Task GetAllGames_ReturnsEmptyResultsForNoAvailabeGames()
        {
            //Arrange
            var results = _fixture.Create<GetGamesResults>();

            _mockGameRepository
                .Setup(exec =>
                    exec.GetAllAvailableGamesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Game>());

            //Act
            var result = await _gameController
                .GetAllGamesAsync() as NotFoundResult;

            //Assert
            _mockGameRepository
                .Verify(exec =>
                    exec.GetAllAvailableGamesAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once);

            result
                .Should()
                .NotBeNull();

            result.StatusCode
                .Should()
                .Be((int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task GetGameByIdAsync_ReturnsGameResultForGivenId()
        {
            //Arrange
            var results = _fixture.Create<GetGameResults>();
            Guid expectedGameId = Guid.Empty;

            var game = _fixture.Create<Game>();

            _mockGameRepository
                .Setup(exec =>
                    exec.GetGameByIdAsync(
                        It.IsAny<Guid>(), 
                        It.IsAny<CancellationToken>()))
                .Callback<Guid, CancellationToken>((gameIdPassed, ct) => 
                {
                    expectedGameId = gameIdPassed; 
                })
                .ReturnsAsync(game);

            //Act
            var result = await _gameController
                .GetGameByIdAsync(game.Id) as OkObjectResult;

            //Assert
            _mockGameRepository
                .Verify(exec =>
                    exec.GetGameByIdAsync(
                            It.IsAny<Guid>(),
                            It.IsAny<CancellationToken>()),
                        Times.Once);

            result
                .Should()
                .NotBeNull();

            result.StatusCode
                .Should()
                .Be((int)HttpStatusCode.OK);

            var gameResult = result.Value as GetGameResults;

            gameResult.Id
                .Should()
                .Be(expectedGameId);
        }

        [TestMethod]
        public async Task GetGameByIdAsync_ReturnsNotFoundResult()
        {
            //Arrange
            _mockGameRepository
                .Setup(exec =>
                    exec.GetGameByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync((Game) null);

            //Act
            var result = await _gameController
                .GetGameByIdAsync(Guid.NewGuid()) as NotFoundResult;

            //Assert
            _mockGameRepository
                .Verify(exec =>
                    exec.GetGameByIdAsync(
                            It.IsAny<Guid>(),
                            It.IsAny<CancellationToken>()),
                        Times.Once);

            result
                .Should()
                .NotBeNull();

            result.StatusCode
                .Should()
                .Be((int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task LetsPlayGameAsync_ReturnsWinningResults()
        {
            //Arrange
            var card = new Card(
                id: Guid.NewGuid(),
                gameId: Guid.NewGuid(),
                faceValue: 54535,
                nextValue: 12432);

            var game = new Game(
                Guid.NewGuid(),
                "Game1",
                card,
                false,
                50,
                DateTime.Now,
                null);

            var request = new PlayGame
			{
                GameId = game.Id,
                FaceValue = card.FaceValue,
                IsNextNumberHigher = false
			};

            _mockGameRepository
                .Setup(exec =>
                    exec.GetGameByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(game);

            //Act
            var result = await _gameController
                .LetsPlayGameAsync(request) as OkObjectResult;

            //Assert
            _mockGameRepository
                .Verify(exec =>
                    exec.GetGameByIdAsync(
                            It.IsAny<Guid>(),
                            It.IsAny<CancellationToken>()),
                        Times.Once);

            result
                .Should()
                .NotBeNull();

            result.StatusCode
                .Should()
                .Be((int)HttpStatusCode.OK);

            var gamePlayResult = result.Value as GamePlayResults;

            gamePlayResult.IsWinner
                .Should()
                .Be(true);
        }

        [TestMethod]
        public async Task LetsPlayGameAsync_ReturnsLostResults()
        {
            //Arrange
            var card = new Card(
                id: Guid.NewGuid(),
                gameId: Guid.NewGuid(),
                faceValue: 5453,
                nextValue: 12432);

            var game = new Game(
                Guid.NewGuid(),
                "Game1",
                card,
                false,
                50,
                DateTime.Now,
                null);

            var request = new PlayGame
            {
                GameId = game.Id,
                FaceValue = card.FaceValue,
                IsNextNumberHigher = false
            };

            _mockGameRepository
                .Setup(exec =>
                    exec.GetGameByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(game);

            //Act
            var result = await _gameController
                .LetsPlayGameAsync(request) as OkObjectResult;

            //Assert
            _mockGameRepository
                .Verify(exec =>
                    exec.GetGameByIdAsync(
                            It.IsAny<Guid>(),
                            It.IsAny<CancellationToken>()),
                        Times.Once);

            result
                .Should()
                .NotBeNull();

            result.StatusCode
                .Should()
                .Be((int)HttpStatusCode.OK);

            var gamePlayResult = result.Value as GamePlayResults;

            gamePlayResult.IsWinner
                .Should()
                .Be(false);
        }

        [TestMethod]
        public async Task LetsPlayGameAsync_ReturnsGameOverIfNoCardsLeft()
        {
            //Arrange
            var card = new Card(
                id: Guid.NewGuid(),
                gameId: Guid.NewGuid(),
                faceValue: 5453,
                nextValue: 12432);

            var game = new Game(
                Guid.NewGuid(),
                "Game1",
                card,
                true,
                0,
                DateTime.Now,
                null);

            var request = new PlayGame
            {
                GameId = game.Id,
                FaceValue = 5453,
                IsNextNumberHigher = false
            };

            _mockGameRepository
                .Setup(exec =>
                    exec.GetGameByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(game);

            //Act
            var result = await _gameController
                .LetsPlayGameAsync(request) as OkObjectResult;

            //Assert
            _mockGameRepository
                .Verify(exec =>
                    exec.GetGameByIdAsync(
                            It.IsAny<Guid>(),
                            It.IsAny<CancellationToken>()),
                        Times.Once);

            result
                .Should()
                .NotBeNull();

            result.StatusCode
                .Should()
                .Be((int)HttpStatusCode.OK);

            var gamePlayResult = result.Value as GamePlayResults;

            gamePlayResult.Message
                .Should()
                .Be("The Game is Over!");
        }

        [TestMethod]
        public async Task LetsPlayGameAsync_ReturnsNotFound()
        {
            //Arrange
            var request = new PlayGame
            {
                GameId = Guid.NewGuid(),
                FaceValue = 5453,
                IsNextNumberHigher = false
            };

            _mockGameRepository
                .Setup(exec =>
                    exec.GetGameByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync((Game) null);

            //Act
            var result = await _gameController
                .LetsPlayGameAsync(request) as NotFoundResult;

            //Assert
            _mockGameRepository
                .Verify(exec =>
                    exec.GetGameByIdAsync(
                            It.IsAny<Guid>(),
                            It.IsAny<CancellationToken>()),
                        Times.Once);

            result
                .Should()
                .NotBeNull();

            result.StatusCode
                .Should()
                .Be((int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task UpdateGame_UpdateGameDataForNextPlayer()
        {
            //Arrange
            _mockGameRepository
                .Setup(exec =>
                    exec.GetGameByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(_fixture.Create<Game>());

            _mockGameRepository
                .Setup(exec =>
                    exec.UpdateGame(
                        It.IsAny<Game>()));

            _mockGameRepository
                .Setup(exec =>
                    exec.CompleteAsync())
                .ReturnsAsync(2);

            //Act
            var result = await _gameController
                .UpdateGame(Guid.NewGuid()) as OkResult;

            //Assert
            _mockGameRepository
                .Verify(exec =>
                    exec.GetGameByIdAsync(
                            It.IsAny<Guid>(),
                            It.IsAny<CancellationToken>()),
                        Times.Once);

            _mockGameRepository
                .Verify(exec =>
                    exec.UpdateGame(
                        It.IsAny<Game>()),
                    Times.Once);

            _mockGameRepository
                .Verify(exec =>
                        exec.CompleteAsync(),
                    Times.Once);

            result
                .Should()
                .NotBeNull();

            result.StatusCode
                .Should()
                .Be((int)HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task UpdateGame_DoesNotUpdateForGameNotAvailabe()
        {
            //Arrange
            _mockGameRepository
                .Setup(exec =>
                    exec.GetGameByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync((Game) null);

            //Act
            var result = await _gameController
                .UpdateGame(Guid.NewGuid()) as NotFoundResult;

            //Assert
            _mockGameRepository
                .Verify(exec =>
                    exec.GetGameByIdAsync(
                            It.IsAny<Guid>(),
                            It.IsAny<CancellationToken>()),
                        Times.Once);

            _mockGameRepository
                .Verify(exec =>
                    exec.UpdateGame(
                        It.IsAny<Game>()),
                    Times.Never);

            _mockGameRepository
                .Verify(exec =>
                        exec.CompleteAsync(),
                    Times.Never);

            result
                .Should()
                .NotBeNull();

            result.StatusCode
                .Should()
                .Be((int)HttpStatusCode.NotFound);
        }
    }
}
