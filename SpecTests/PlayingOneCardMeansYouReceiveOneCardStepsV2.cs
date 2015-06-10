using System;
using TechTalk.SpecFlow;

namespace SpecTests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using FluentAssertions;

    using Palace;

    using SpecTests.Helpers;

    [Binding]
    public class PlayingOneCardMeansYouReceiveOneCardStepsV2
    {
        private ICollection<Player> players;

        private Player currentPlayer;

        private Dealer dealer;

        private Game game;
         [Given(@"I have the following players and cards")]
        public void GivenIHaveTheFollowingPlayersAndCards(Table table)
        {
            players = new Collection<Player>();
            foreach (var row in table.Rows)
            {
                string name;
                string cardsString;
                row.TryGetValue("Player", out name);
                row.TryGetValue("CardsInHand", out cardsString);
                var cards = GetCardsFromCsvString(cardsString);
                var player = PlayerHelper.CreatePlayer(cards, name);
                players.Add(player);
            }
             dealer = DealerHelper.TestDealer(players);
             game = dealer.StartGame();
        }

        [When(@"'(.*)' plays the '(.*)'")]
        public void WhenPlaysThe(string playerName, string card)
        {
            currentPlayer = players.First(p => p.Name.Equals(playerName));
            game.PlayInHandCards(currentPlayer, GetCardFromStringValue(card));
        }


        [Then(@"'(.*)' should be have three cards")]
        public void ThenShouldBeHaveThreeCards(string playerName)
        {
            var playerForAssertion = players.First(p => p.Name.Equals(playerName));
            playerForAssertion.NumCardsInHand.Should().Be(3);
        }



        public List<Card> GetCardsFromCsvString(string csvString)
        {
            var cardsSplit = csvString.Replace(" ", string.Empty).Split(',');
            var cards = cardsSplit.Select(GetCardFromStringValue).ToList();
            return cards;
        }

        private Card GetCardFromStringValue(string card)
        {
            switch (card)
            {
                case "TenOfClubs":
                    return Card.TenOfClubs;
                case "FourOfClubs":
                    return Card.FourOfClubs;
                case "AceOfClubs":
                    return Card.AceOfClubs;
                default: throw new Exception();
            }
        }
    }

    
}
