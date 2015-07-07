namespace Palace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RulesProcessesor
    {
        private Guid gameId;

        private Dictionary<CardValue, RuleForCard> _rulesForCardsByValue;

        public RulesProcessesor(Guid gameId, Dictionary<CardValue, RuleForCard> rulesForCardsByValue)
        {
            this.GameId = gameId;
            this.RulesForCardsByValue = rulesForCardsByValue;
        }

        public Guid GameId
        {
            get
            {
                return this.gameId;
            }
            set
            {
                this.gameId = value;
            }
        }

        internal Dictionary<CardValue, RuleForCard> RulesForCardsByValue
        {
            get
            {
                return this._rulesForCardsByValue;
            }
            set
            {
                this._rulesForCardsByValue = value;
            }
        }

        internal RuleChecker GetRuleChecker(GameState state, IEnumerable<Card> cardsPlayed)
        {
            return new RuleChecker(this.RulesForCardsByValue, state, cardsPlayed);
        }
    }

    //Todo: Rename!
    internal class RuleChecker
    {
        private readonly Dictionary<CardValue, RuleForCard> _rulesForCardsByValue;

        private readonly GameState state;

        private readonly IEnumerable<Card> cardsPlayed;

        public RuleChecker(Dictionary<CardValue, RuleForCard> rulesForCardsByValue, GameState state, IEnumerable<Card> cardsPlayed)
        {
            this._rulesForCardsByValue = rulesForCardsByValue;
            this.state = state;
            this.cardsPlayed = cardsPlayed;
        }

        internal bool CardCanBePlayed()
        {
            return this.CheckCardCanBePlayed(cardsPlayed.First(), state.PlayPile);
        }

        internal bool PlayPileShouldBeCleared()
        {
            return this.ShouldBurn(state.PlayPile);
        }

        internal LinkedListNode<Player> SetNextPlayer()
        {
            if (cardsPlayed == null)
            {
                this.GetNextPlayerFromOrderOfPlay(state.OrderOfPlay, state.CurrentPlayerLinkedListNode);
                return state.CurrentPlayerLinkedListNode;
            }

            if (ShouldBurn(state.PlayPileStack))
                return state.CurrentPlayerLinkedListNode;

            var ruleForPlayersCard = this.GetRuleForCardFromCardValue(cardsPlayed.First().Value);
            var nextPayer = this.GetNextPlayerFromOrderOfPlay(state.OrderOfPlay, state.CurrentPlayerLinkedListNode);

            if (ruleForPlayersCard != RuleForCard.SkipPlayer)
                return nextPayer;

            var skipCardValue = this._rulesForCardsByValue.First(w => w.Value == RuleForCard.SkipPlayer).Key;
            var topCardsInPlayPileWithSkipValue = cardsPlayed.GetTopCardsWithSameValue(skipCardValue);

            foreach (var card in topCardsInPlayPileWithSkipValue)
                nextPayer = this.GetNextPlayerFromOrderOfPlay(state.OrderOfPlay, nextPayer);

            return nextPayer;
        }

        internal OrderOfPlay GetOrderOfPlay()
        {
            var cardToPlay = cardsPlayed.First();
            var rulesForPlayersCard = this.GetRuleForCardFromCardValue(cardToPlay.Value);
            if (rulesForPlayersCard == RuleForCard.ReverseOrderOfPlay)
                return state.OrderOfPlay == OrderOfPlay.Forward ? OrderOfPlay.Backward : OrderOfPlay.Forward;

            return state.OrderOfPlay;
        }

        private LinkedListNode<Player> GetNextPlayerFromOrderOfPlay(OrderOfPlay orderOfPlay, LinkedListNode<Player> ppp)
        {
            if (orderOfPlay == OrderOfPlay.Forward)
                return ppp.Next ?? state.Players.First;
            
            return ppp.Previous ?? state.Players.Last;
        }

        private bool ShouldBurn(IEnumerable<Card> cardsToCheck)
        {
            cardsToCheck = cardsToCheck as IList<Card> ?? cardsToCheck.ToList();
            if (!cardsToCheck.Any()) return false;

            var lastFourCardsAreSameValue = cardsToCheck.GetTopCardsWithSameValue(cardsToCheck.First().Value).Count() >= 4;
            var isBurnCard = this.GetRuleForCardFromCardValue(cardsToCheck.First().Value) == RuleForCard.Burn;

            return isBurnCard || lastFourCardsAreSameValue;
        }

        private bool CheckCardCanBePlayed(Card cardToPlay, IEnumerable<Card> cards)
        {
            var playPileAsList = cards as IList<Card> ?? cards.ToList();

            if (!playPileAsList.Any())
                return true;

            var lastCardPlayed = playPileAsList.First();

            var rulesForPlayersCard = this.GetRuleForCardFromCardValue(cardToPlay.Value);
            var ruleForLastCardPlayed = this.GetRuleForCardFromCardValue(lastCardPlayed.Value);

            if (ruleForLastCardPlayed == RuleForCard.SeeThrough)
            {
                return CheckCardCanBePlayed(cardToPlay, playPileAsList.Except(new[] { lastCardPlayed }));
            }

            if (rulesForPlayersCard == RuleForCard.Reset || rulesForPlayersCard == RuleForCard.Burn || rulesForPlayersCard == RuleForCard.SeeThrough)
                return true;
            if (ruleForLastCardPlayed == RuleForCard.Standard && cardToPlay.Value < lastCardPlayed.Value)
                return false;
            if (ruleForLastCardPlayed == RuleForCard.LowerThan && cardToPlay.Value > lastCardPlayed.Value)
                return false;

            return true;
        }

        private RuleForCard GetRuleForCardFromCardValue(CardValue cardValue)
        {
            RuleForCard ruleForCard;
            this._rulesForCardsByValue.TryGetValue(cardValue, out ruleForCard);
            return ruleForCard == 0 ? RuleForCard.Standard : ruleForCard;
        }
    }
}