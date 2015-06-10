namespace SpecTests
{
    using FluentAssertions;

    using Palace;

    using SpecTests.Helpers;

    using TechTalk.SpecFlow;

    [Binding]
    public class PlayingOneCardMeansYouReceiveOneCardSteps
    {
        private Player player1;

        private Player player2;

        private Dealer dealer;

        private Game game;

        [Given(@"I have two players, called Ed and Sophie, with three cards each in a game in progress")]
        public void GivenIHaveTwoPlayersCalledEdAndSophieWithThreeCardsEachInAGameInProgress()
        {
            player1 = PlayerHelper.CreatePlayer(new[] { Card.TwoOfClubs, Card.SevenOfClubs, Card.AceOfClubs }, "Ed");
            player2 = PlayerHelper.CreatePlayer(new[] { Card.ThreeOfClubs, Card.EightOfClubs, Card.TenOfClubs }, "Sophie");
            dealer = DealerHelper.TestDealer(new[] { player1, player2 });
            game = dealer.StartGame();
        }

        [When(@"Ed plays a card")]
        public void WhenEdPlaysACard()
        {
            game.PlayInHandCards(player1, Card.TwoOfClubs);
        }

        [Then(@"Ed should be given a card from the deck")]
        public void ThenEdShouldBeGivenACardFromTheDeck()
        {
            player1.NumCardsInHand.Should().Be(3);
        }
    }
}