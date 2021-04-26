namespace HigherOrLower.Tests.Infrastructure
{
	using System.Linq;
	using AutoFixture;
	using FluentAssertions;
	using HigherOrLower.Domain.Game;
	using HigherOrLower.Infrastructure.Entities;
	using HigherOrLower.Infrastructure.Mapper;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class GameMapperTests
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
		public void ToDomain_WithValidGameEntity_ShouldBeExecuteCorrectly()
		{
			// Arrange
			var gameEntity = _fixture.Create<GameEntity>();

			var expected = new Game(
				id: gameEntity.GameId,
				name: gameEntity.Name,
				card: gameEntity.CardEntity.ToDomain(),
				isGameOver: gameEntity.IsGameOver,
				remainingCards: gameEntity.RemainingCards,
				createdAt: gameEntity.CreatedAt,
				updatedAt: gameEntity.UpdatedAt);

			// Act
			var domainResult = gameEntity.ToDomain();

			// Assert
			domainResult
				.Should()
				.BeEquivalentTo(expected);
		}

		[TestMethod]
		public void ToDomain_WithNullGameEntity_ShoulReturnNull()
		{
			// Arrange
			var expected = null as GameEntity;

			// Act
			var domainResult = expected.ToDomain();

			// Assert
			domainResult
				.Should()
				.BeNull();
		}

		[TestMethod]
		public void ToEntity_WithValidGameDomain_ShouldBeExecuteCorrectly()
		{
			// Arrange
			var game = _fixture.Create<Game>();

			var expected = new GameEntity
			{
				GameId = game.Id,
				Name = game.Name,
				RemainingCards = game.RemainingCards,
				IsGameOver = game.IsGameOver,
				CreatedAt = game.CreatedAt,
				UpdatedAt = game.UpdatedAt,
				CardEntity = game.Card.ToEntity()
			};

			// Act
			var entityResult = game.ToEntity();

			// Assert
			entityResult
				.Should()
				.BeEquivalentTo(expected);
		}

		[TestMethod]
		public void ToEntity_WithNullGameDomain_ShoulReturnNull()
		{
			// Arrange
			var expected = null as Game;

			// Act
			var entityResult = expected.ToEntity();

			// Assert
			entityResult
				.Should()
				.BeNull();
		}
	}
}
