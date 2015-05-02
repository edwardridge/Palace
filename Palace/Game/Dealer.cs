using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace
{
    public class Dealer : IRulesValidator
    {
        public Dealer(Deck deck, ICanStartGame canStartGame)
            : this(deck, canStartGame, new Dictionary<CardValue, RuleForCard>())
        {

        }

        public Dealer(Deck deck, ICanStartGame canStartGame, Dictionary<CardValue, RuleForCard> rulesForCardsByValue)
        {
            this._deck = deck;
            this._rulesForCardsByValue = rulesForCardsByValue;
            this.canStartGame = canStartGame;
        }

        public void DealIntialCards(ICollection<IPlayer> players)
        {
            foreach (var player in players)
            {
                player.AddCards(_deck.DealCards(3, CardOrientation.FaceDown));
                player.AddCards(_deck.DealCards(6, CardOrientation.InHand));
            }
        }

        public Game StartGame(ICollection<IPlayer> players, IPlayer startingPlayer = null)
        {
            if (!this.canStartGame.GameIsReadyToStart(players))
                throw new InvalidOperationException("The game is not ready to start");

            var game = new Game(players, this, _deck);
            if (startingPlayer == null)
            {
                startingPlayer = players.First();

                foreach (var player in players)
                {
                    if (player.Cards == null || player.Cards.Count == 0)
                        continue;
                    if (player.LowestCardInValue.Value < startingPlayer.LowestCardInValue.Value)
                        startingPlayer = player;
                }
            }

            game.Start(startingPlayer);
            return game;
        }

        public Game StartGameWithPlayPile(ICollection<IPlayer> players, IPlayer startingPlayer, IEnumerable<Card> cardsInPile)
        {
            var game = new Game(players, this, _deck, cardsInPile);
            game.Start(startingPlayer);
            return game;
        }

        public bool CardsPassRules(IEnumerable<Card> cardsToPlay, Card lastCardPlayed)
        {
            var cardsList = cardsToPlay as IList<Card> ?? cardsToPlay.ToList();

            if (lastCardPlayed == null)
                return true;

            var playersCard = cardsList.First();
            var ruleForLastCardPlayed = this.getRuleForCardFromCardValue(lastCardPlayed.Value);
            var rulesForPlayersCard = this.getRuleForCardFromCardValue(playersCard.Value);

            if (ruleForLastCardPlayed == RuleForCard.Reset || rulesForPlayersCard == RuleForCard.Reset)
                return true;
            if (ruleForLastCardPlayed == RuleForCard.Standard && playersCard.Value < lastCardPlayed.Value)
                return false;
            if (ruleForLastCardPlayed == RuleForCard.LowerThan && playersCard.Value > lastCardPlayed.Value)
                return false;
            

            return true;
        }

        private RuleForCard getRuleForCardFromCardValue(CardValue cardValue)
        {
            RuleForCard ruleForCard;
            _rulesForCardsByValue.TryGetValue(cardValue, out ruleForCard);
            return ruleForCard == 0 ? RuleForCard.Standard : ruleForCard;
        }

        private Deck _deck;
        private Dictionary<CardValue, RuleForCard> _rulesForCardsByValue;
        private ICanStartGame canStartGame;
    }
}
