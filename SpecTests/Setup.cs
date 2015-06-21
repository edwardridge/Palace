namespace SpecTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Palace;

    using TechTalk.SpecFlow;

    using TestHelpers;

    [Binding]
    public class Setup
    {
        private ICollection<Player> players;

        private Player currentPlayer;

        private Dealer dealer;

        private Game game;

        private Dictionary<CardValue, RuleForCard> ruleForCardsByValue;

        [Given(@"I have the following players and cards")]
        public void GivenIHaveTheFollowingPlayersAndCards(Table table)
        {
            players = this.GetPlayersFromTable(table);
            ruleForCardsByValue = new Dictionary<CardValue, RuleForCard>();
            dealer = DealerHelper.TestDealer(players);
            game = dealer.StartGame();
            ScenarioContext.Current.Set(game);
        }

        [Given(@"it is '(.*)' turn")]
        public void GivenItIsTurn(string playerName)
        {
            var playerToStart = players.First(f => f.Name.Equals(playerName));
            dealer = DealerHelper.TestDealer(players);
            game = dealer.StartGame(playerToStart);
            ScenarioContext.Current.Set(game);
        }

        [Given(@"the following cards have rules")]
        public void GivenTheFollowingCardsHaveRules(Table table)
        {
            foreach (var row in table.Rows)
            {
                string cardValueAsString;
                string ruleForCardAsString;
                row.TryGetValue("CardValue", out cardValueAsString);
                row.TryGetValue("Rule", out ruleForCardAsString);
                ruleForCardsByValue.Add(GetCardValueFromStringValue(cardValueAsString), GetRuleForCardFromStringValue(ruleForCardAsString));
            }

            dealer = DealerHelper.TestDealerWithRules(players, ruleForCardsByValue);
            game = dealer.StartGame();
            ScenarioContext.Current.Set(game);
        }

        [Given(@"the deck has no more cards")]
        public void GivenTheDeckHasNoMoreCards()
        {
            var remainingCards = new List<Card>();
            dealer = new Dealer(players, new PredeterminedDeck(remainingCards), new DummyCanStartGame(), ruleForCardsByValue);
            game = dealer.StartGame();
            ScenarioContext.Current.Set(game);
        }

        [When(@"The game starts")]
        public void WhenTheGameStarts()
        {
            // dealer = ScenarioContext.Current.Get<Dealer>();
            game = dealer.StartGame();
            ScenarioContext.Current.Set(game);
        }

        [When(@"'(.*)' plays the '(.*)'")]
        public void WhenPlaysThe(string playerName, string card)
        {
            currentPlayer = game.Players.First(p => p.Name.Equals(playerName));
            var result = game.PlayInHandCards(currentPlayer, GetCardFromStringValue(card));
            ScenarioContext.Current.Set(result);
        }

        [When(@"'(.*)' plays the face down card '(.*)'")]
        public void WhenPlaysTheFaceDownCard(string playerName, string card)
        {
            currentPlayer = players.First(p => p.Name.Equals(playerName));
            var result = game.PlayFaceDownCards(currentPlayer, GetCardFromStringValue(card));
            ScenarioContext.Current.Set(result);
        }

        [When(@"'(.*)' plays the face up card '(.*)'")]
        public void WhenPlaysTheFaceUpCard(string playerName, string card)
        {
            currentPlayer = players.First(p => p.Name.Equals(playerName));
            var result = game.PlayFaceUpCards(currentPlayer, GetCardFromStringValue(card));
            ScenarioContext.Current.Set(result);
        }

        private List<Player> GetPlayersFromTable(Table table)
        {
            var playersFromTable = new List<Player>();
            foreach (var row in table.Rows)
            {
                string name;
                string cardsInHandString;
                string cardsFaceDownString;
                string cardsFaceUpString;

                row.TryGetValue("Player", out name);
                row.TryGetValue("CardsInHand", out cardsInHandString);
                row.TryGetValue("CardsFaceDown", out cardsFaceDownString);
                row.TryGetValue("CardsFaceUp", out cardsFaceUpString);

                var cardsInHand = this.GetCardsFromCsvString(cardsInHandString);

                var cardsFaceUp = new List<Card>();
                if (cardsFaceUpString != null)
                    cardsFaceUp = this.GetCardsFromCsvString(cardsFaceUpString);

                var cardsFaceDown = new List<Card>();
                if (cardsFaceDownString != null)
                    cardsFaceDown = this.GetCardsFromCsvString(cardsFaceDownString);

                var player = PlayerHelper.CreatePlayer(cardsInHand.Concat(cardsFaceUp), name, cardsFaceDown);
                foreach (var card in cardsFaceUp)
                {
                    player.PutCardFaceUp(card);
                }

                playersFromTable.Add(player);
            }

            return playersFromTable;
        }

        private List<Card> GetCardsFromCsvString(string csvString)
        {
            if (string.IsNullOrEmpty(csvString))
                return new List<Card>();
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
                case "NineOfClubs":
                    return Card.NineOfClubs;
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
                default:
                    throw new Exception("Card in test does not map");
            }
        }

        private CardValue GetCardValueFromStringValue(string cardValue)
        {
            switch (cardValue)
            {
                case "Two":
                    return CardValue.Two;
                case "Three":
                    return CardValue.Three;
                case "Four":
                    return CardValue.Four;
                case "Five":
                    return CardValue.Five;
                case "Six":
                    return CardValue.Six;
                case "Seven":
                    return CardValue.Seven;
                case "Eight":
                    return CardValue.Eight;
                case "Nine":
                    return CardValue.Nine;
                case "Ten":
                    return CardValue.Ten;
                case "Jack":
                    return CardValue.Jack;
                case "Queen":
                    return CardValue.Queen;
                case "King":
                    return CardValue.King;
                case "AceOfClubs":
                    return CardValue.Ace;
                default:
                    throw new Exception("Card in test does not map");
            }
        }

        private RuleForCard GetRuleForCardFromStringValue(string ruleForCard)
        {
            switch (ruleForCard)
            {
                case "Reset":
                    return RuleForCard.Reset;
                default:
                    throw new Exception("Rule for card in test not found");
            }
        }
    }
}