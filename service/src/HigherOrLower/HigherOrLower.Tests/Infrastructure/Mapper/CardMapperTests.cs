namespace HigherOrLower.Tests.Infrastructure
{
	using System.Linq;
	using AutoFixture;
	using FluentAssertions;
	using HigherOrLower.Domain.Card;
	using HigherOrLower.Infrastructure.Entities;
	using HigherOrLower.Infrastructure.Mapper;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class CardMapperTests
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
		public void ToDomain_WithValidCard_ShouldBeExecuteCorrectly()
		{
			// Arrange
			var cardEntity = _fixture.Create<CardEntity>();

			var expected = new Card(
				id: cardEntity.CardId,
				gameId: cardEntity.GameId,
				faceValue: cardEntity.FaceValue,
				nextValue: cardEntity.NextValue);

			// Act
			var domainResult = cardEntity.ToDomain();

			// Assert
			domainResult
				.Should()
				.BeEquivalentTo(expected);
		}

		[TestMethod]
		public void ToDomain_WithNullCard_ShoulReturnNull()
		{
			// Arrange
			var expected = null as CardEntity;

			// Act
			var domainResult = expected.ToDomain();

			// Assert
			domainResult
				.Should()
				.BeNull();
		}

		[TestMethod]
		public void ToEntity_WithValidCard_ShouldBeExecuteCorrectly()
		{
			// Arrange
			var card = _fixture.Create<Card>();

			var expected = new CardEntity
			{
				CardId = card.Id,
				GameId = card.GameId,
				FaceValue = card.FaceValue,
				NextValue = card.NextValue
			};

			// Act
			var entityResult = card.ToEntity();

			// Assert
			entityResult
				.Should()
				.BeEquivalentTo(expected);
		}

		[TestMethod]
		public void ToEntity_WithNullCard_ShoulReturnNull()
		{
			// Arrange
			var expected = null as Card;

			// Act
			var entityResult = expected.ToEntity();

			// Assert
			entityResult
				.Should()
				.BeNull();
		}
	}
}
