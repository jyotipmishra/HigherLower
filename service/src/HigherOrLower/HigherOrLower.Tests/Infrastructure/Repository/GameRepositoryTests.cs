namespace HigherOrLower.Tests.Infrastructure.Repository
{
	using System.Linq;
	using System.Threading.Tasks;
	using AutoFixture;
	using FluentAssertions;
	using HigherOrLower.Domain.Game;
	using HigherOrLower.Infrastructure.Entities;
	using HigherOrLower.Infrastructure.Mapper;
	using HigherOrLower.Infrastructure.Repositories;
	using HigherOrLower.Infrastructure.Tests.Repositories;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class GameRepositoryTests
	{
        private Fixture _fixture;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [TestMethod]
        public async Task CreateGameAsync_StoreNewGameInDb()
		{
            //Arrange
            var newGame = _fixture.Create<Game>();

            var dbContext = InMemoryDbContext.CreateDbContext();

            var gameRepository = new GameRepository(dbContext);

            //Act
            await gameRepository.CreateGameAsync(newGame);
            await dbContext.SaveChangesAsync();

            //Assert
            var expected = dbContext.Games.FirstOrDefault();
            
            expected
                .Should()
                .NotBeNull();
        }

        [TestMethod]
        public async Task GetGameByIdAsync_GetGameForTheIdPassed()
        {
            //Arrange
            var newGame = _fixture
                .Build<GameEntity>()
                .With(g => g.IsGameOver, false)
                .Create();

            var dbContext = InMemoryDbContext.CreateDbContext();

            var gameRepository = new GameRepository(dbContext);

            await dbContext.AddAsync(newGame);
            await dbContext.SaveChangesAsync();

            //Act
            var result = await gameRepository.GetGameByIdAsync(newGame.GameId);

            //Assert
            result
                .Should()
                .NotBeNull();
        }

        [TestMethod]
        public async Task GetAllGamesAsync_ShouldReturnAllGamesAvailabeToPlay()
        {
            //Arrange
            var newGames = _fixture
                .Build<GameEntity>()
                .With(x => x.IsGameOver, false)
                .CreateMany(5);

            var dbContext = InMemoryDbContext.CreateDbContext();

            var gameRepository = new GameRepository(dbContext);

            await dbContext.AddRangeAsync(newGames);
            await dbContext.SaveChangesAsync();

            //Act
            var result = await gameRepository.GetAllAvailableGamesAsync();

            //Assert
            result
                .Should()
                .NotBeNull();

            result.Count()
                .Should()
                .Be(5);
        }

        [TestMethod]
        public async Task GetAllGamesAsync_ShouldReturnZeroGamesAvailabeToPlay()
        {
            //Arrange
            var newGames = _fixture
                .Build<GameEntity>()
                .With(x => x.IsGameOver, true)
                .CreateMany(5);

            var dbContext = InMemoryDbContext.CreateDbContext();

            var gameRepository = new GameRepository(dbContext);

            await dbContext.AddRangeAsync(newGames);
            await dbContext.SaveChangesAsync();

            //Act
            var result = await gameRepository.GetAllAvailableGamesAsync();

            //Assert
            result
                .Should()
                .NotBeNull();

            result.Count()
                .Should()
                .Be(0);
        }

        [TestMethod]
        public async Task UpdateGame_ShouldUpdateNumberOfCardsRemainingAfterEachRound()
        {
            //Arrange
            var newGame = _fixture.Create<Game>();

            var gameEntity = newGame.ToEntity();
            
            var valueBeforeUpdate = newGame.RemainingCards; 
            
            var dbContext = InMemoryDbContext.CreateDbContext();
            await dbContext.AddAsync(gameEntity);
            await dbContext.SaveChangesAsync();

            dbContext.Entry(gameEntity).State = EntityState.Detached;

            newGame.PrepareForNextPlayer();

            //Act
            var gameRepository = new GameRepository(dbContext);
            gameRepository.UpdateGame(newGame);
            await dbContext.SaveChangesAsync();

            //Assert
            var updatedGame = dbContext.Games.FirstOrDefault();

            valueBeforeUpdate
                .Should()
                .NotBe(updatedGame.RemainingCards);
        }
    }
}
