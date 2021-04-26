namespace HigherOrLower.Tests.Domain
{
	using System;
	using AutoFixture;
	using HigherOrLower.Domain.Card;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class CardTests
	{
		private Fixture _fixture;

		[TestInitialize]
		public void TestInitialize()
		{
			_fixture = new Fixture();
		}

		[TestMethod]
		public void Card_ShouldReturnCorrectly()
		{
			// Arrange
			var cardId = Guid.NewGuid();
			var gameId = Guid.NewGuid();

			// Act
			var expectedGame = Card.CreateNew(
				cardId,
				gameId);

			// Assert
			Assert.AreEqual(expectedGame.Id, cardId);
			Assert.AreEqual(expectedGame.GameId, gameId);
			Assert.IsTrue(expectedGame.FaceValue >= 0);
			Assert.IsTrue(expectedGame.NextValue >= 0);
		}

		[TestMethod]
		public void SetANewNumber_ShouldReturnANewNumber()
		{
			// Arrange
			var card = _fixture.Create<Card>();
			var faceValueBefore = card.FaceValue;
			var nextValueBefore = card.NextValue;

			// Act
			card.SetANewNumber();

			// Assert
			Assert.AreNotEqual(nextValueBefore, card.NextValue);
			Assert.AreNotEqual(faceValueBefore, card.FaceValue);
			Assert.AreEqual(nextValueBefore, card.FaceValue);
		}
	}
}
