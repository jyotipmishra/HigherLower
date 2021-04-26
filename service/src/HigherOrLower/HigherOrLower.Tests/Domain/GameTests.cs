namespace HigherOrLower.Tests.Domain
{
	using System;
	using AutoFixture;
	using HigherOrLower.Domain.Card;
	using HigherOrLower.Domain.Game;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class GameTests
	{
		private Fixture _fixture;

		[TestInitialize]
		public void TestInitialize()
		{
			_fixture = new Fixture();
		}

		[TestMethod]
		public void Game_ShouldReturnCorrectly()
		{
			// Arrange
			var gameId = Guid.NewGuid();
			var name = "Game 1";
			var card = _fixture.Create<Card>();
			var numberOfCards = 5;

			// Act
			var expectedGame = Game.CreateNew(
				id: gameId,
				name: name,
				card: card,
				numberOfCards: numberOfCards);

			// Assert
			Assert.AreEqual(expectedGame.Id, gameId);
			Assert.AreEqual(expectedGame.Name, name);
			Assert.AreEqual(expectedGame.Card, card);
			Assert.AreEqual(expectedGame.RemainingCards, numberOfCards - 1);
		}

		[TestMethod]
		public void Game_ShouldReturnCorrectlyWithDefaultNumberOfCards_WithNull()
		{
			// Arrange
			var gameId = Guid.NewGuid();
			var name = "Game 1";
			var card = _fixture.Create<Card>();

			// Act
			var expectedGame = Game.CreateNew(
				id: gameId,
				name: name,
				card: card,
				numberOfCards: null);

			// Assert
			Assert.AreEqual(expectedGame.Id, gameId);
			Assert.AreEqual(expectedGame.Name, name);
			Assert.AreEqual(expectedGame.Card, card);
			Assert.IsNotNull(expectedGame.RemainingCards);
		}

		[TestMethod]
		public void PrepareForNextPlayer_ShouldReturnReturnOneLessThanCurrentNumber()
		{
			// Arrange
			var game = _fixture.Create<Game>();
			var cardsBefore = game.RemainingCards;

			// Act
			game.PrepareForNextPlayer();

			// Assert
			Assert.IsTrue(cardsBefore > game.RemainingCards);
		}

		[TestMethod]
		public void PrepareForNextPlayer_ShouldSetIsOverToTrueForZeroCards()
		{
			// Arrange
			var game = new Game(
				id: _fixture.Create<Guid>(),
				name: _fixture.Create<string>(),
				card: _fixture.Create<Card>(),		
				isGameOver: false,
				remainingCards: 1,
				createdAt: _fixture.Create<DateTime>(),
				updatedAt: _fixture.Create<DateTime>());

			var cardsBefore = game.RemainingCards;
			var isOverBefore = game.IsGameOver;

			// Act
			game.PrepareForNextPlayer();

			// Assert
			Assert.IsTrue(cardsBefore > game.RemainingCards);
			Assert.AreNotEqual(isOverBefore, game.IsGameOver);
		}
	}
}
