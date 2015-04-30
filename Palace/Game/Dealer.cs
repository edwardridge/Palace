using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Palace
{
    public class Dealer : IRulesValidator
    {
        public Dealer(Deck deck, IGameStartValidator gameStartValidator)
            : this(deck, gameStartValidator, new Dictionary<CardValue, RuleForCard>())
        {

        }

        public Dealer(Deck deck, IGameStartValidator gameStartValidator, Dictionary<CardValue, RuleForCard> rulesForCardsByValue)
        {
            this._deck = deck;
            this._rulesForCardsByValue = rulesForCardsByValue;
            this._gameStartValidator = gameStartValidator;
        }

        public void DealIntialCards(ICollection<IPlayer> players)
        {
            foreach (var player in players)
            {
                player.AddCards(_deck.TakeCards(3, CardOrientation.FaceDown));
                player.AddCards(_deck.TakeCards(6, CardOrientation.InHand));
            }
        }

        public Game StartGame(ICollection<IPlayer> players)
        {
            if (!_gameStartValidator.GameIsReadyToStart(players))
                throw new InvalidOperationException();

            var game = new Game(players, this);

            var startingPlayer = players.First();

            foreach (var player in players)
            {
                if (player.Cards == null || player.Cards.Count == 0)
                    continue;
                if (player.LowestCardInValue.Value < startingPlayer.LowestCardInValue.Value)
                    startingPlayer = player;
            }

            game.Start(startingPlayer);
            return game;
        }

        public Game ResumeGame(ICollection<IPlayer> players, IPlayer startingPlayer)
        {
            return ResumeGame(players, startingPlayer, new List<Card>());
        }

        public Game ResumeGame(ICollection<IPlayer> players, IPlayer startingPlayer, IEnumerable<Card> cardsInPile)
        {
            var game = new Game(players, this);
            game.Start(startingPlayer);
            return game;
        }

        public bool CardsPassRules(IEnumerable<Card> cardsToPlay, Card lastCardPlayed)
        {
            var cardsList = cardsToPlay as IList<Card> ?? cardsToPlay.ToList();

            if (lastCardPlayed == null)
                return true;

            var playersCard = cardsList.First();
            var ruleForCard = this.getRuleForCardFromCardValue(playersCard.Value);

            if (ruleForCard == RuleForCard.Standard && playersCard.Value < lastCardPlayed.Value)
                return false;
            if (ruleForCard == RuleForCard.LowerThan && playersCard.Value > lastCardPlayed.Value)
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
        private IGameStartValidator _gameStartValidator;
    }
}
