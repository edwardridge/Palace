using System;
using TechTalk.SpecFlow;

namespace SpecTests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Media;

    using FluentAssertions;

    using Palace;

    using SpecTests.Helpers;

    [Binding]
    public class Setup
    {
        
        private ICollection<Player> players;

        private Player currentPlayer;

        private Dealer dealer;

        private Game game;

        private ResultWrapper resultWrapper;

        private ResultOutcome result;

        public Setup(ResultWrapper resultWrapper)
        {
            this.resultWrapper = resultWrapper;
            //this.game = game;
        }

         [Given(@"I have the following players and cards")]
        public void GivenIHaveTheFollowingPlayersAndCards(Table table)
        {
            players = new List<Player>();
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
             ScenarioContext.Current.Add("game", game);
             ScenarioContext.Current.Add("dealer", dealer);
        }

         [Given(@"it is '(.*)' turn")]
         public void GivenItIsTurn(string playerName)
         {
             var playerToStart = players.First(f => f.Name.Equals(playerName));
             dealer = DealerHelper.TestDealer(players);
             game = dealer.StartGame(playerToStart);
         }

        [When(@"'(.*)' plays the '(.*)'")]
        public void WhenPlaysThe(string playerName, string card)
        {
            currentPlayer = players.First(p => p.Name.Equals(playerName));
            result = game.PlayInHandCards(currentPlayer, GetCardFromStringValue(card));
            resultWrapper.resultOutcome = result;

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
                case "TwoOfClubs":
                    return Card.TwoOfClubs;
                case "ThreeOfClubs":
                    return Card.ThreeOfClubs;
                case "FourOfClubs":
                    return Card.FourOfClubs;
                case "FiveOfClubs":
                    return Card.FiveOfClubs;
                case "SixOfClubs":
                    return Card.SixOfClubs;
                case "SevenOfClubs":
                    return Card.SevenOfClubs;
                case "EightOfClubs":
                    return Card.EightOfClubs;
                case "TenOfClubs":
                    return Card.TenOfClubs;
                case "JackOfClubs":
                    return Card.JackOfClubs;
                case "QueenOfClubs":
                    return Card.JackOfClubs;
                case "KingOfClubs":
                    return Card.JackOfClubs;
                case "AceOfClubs":
                    return Card.AceOfClubs;
                default: throw new Exception();
            }
        }
    }

    public class ResultWrapper
    {
        public ResultOutcome resultOutcome;

        public ResultWrapper()
        {
        }
    }

    
}
