namespace Palace
{
    using System.Collections.Generic;
    using System.Linq;

    public class RulesProcessesor
    {
        private Dictionary<CardValue, RuleForCard> _rulesForCardsByValue;

        public RulesProcessesor(Dictionary<CardValue, RuleForCard> rulesForCardsByValue)
        {
            this._rulesForCardsByValue = rulesForCardsByValue;
        }

        private Dictionary<CardValue, RuleForCard> RulesForCardsByValue
        {
            get
            {
                return this._rulesForCardsByValue;
            }
        }

        internal bool CardCanBePlayed(Card cardToPlay, IEnumerable<Card> playPile)
        {
            var playPileAsList = playPile as IList<Card> ?? playPile.ToList();

            if (!playPileAsList.Any())
                return true;

            var lastCardPlayed = playPileAsList.First();

            var rulesForPlayersCard = this.GetRuleForCardFromCardValue(cardToPlay.Value);
            var ruleForLastCardPlayed = this.GetRuleForCardFromCardValue(lastCardPlayed.Value);

            if (ruleForLastCardPlayed == RuleForCard.SeeThrough)
            {
                var playPileExceptLastCard = playPileAsList.Except(new[]{lastCardPlayed});
                return CardCanBePlayed(cardToPlay, playPileExceptLastCard);
            }
            
            if (rulesForPlayersCard == RuleForCard.Reset || rulesForPlayersCard == RuleForCard.Burn || rulesForPlayersCard == RuleForCard.SeeThrough)
                return true;
            if (ruleForLastCardPlayed == RuleForCard.Standard && cardToPlay.Value < lastCardPlayed.Value)
                return false;
            if (ruleForLastCardPlayed == RuleForCard.LowerThan && cardToPlay.Value > lastCardPlayed.Value)
                return false;

            return true;
        }

        internal OrderOfPlay SetOrderOfPlay(GameState state, Card cardToPlay)
        {
            var rulesForPlayersCard = this.GetRuleForCardFromCardValue(cardToPlay.Value);
            if (rulesForPlayersCard == RuleForCard.ReverseOrderOfPlay)
                return state.OrderOfPlay == OrderOfPlay.Forward ? OrderOfPlay.Backward : OrderOfPlay.Forward;
            else
                return state.OrderOfPlay;
        }

        internal bool PlayPileShouldBeCleared(IEnumerable<Card> playPile)
        {
            return this.ShouldBurn(playPile);
        }

        internal void SetNextPlayer(
            IEnumerable<Card> cardsPlayed,
            GameState gameState)
        {
            if (cardsPlayed == null)
            {
                this.SetPlayerFromOrderOfPlay(gameState);
                return;
            }

            cardsPlayed = cardsPlayed as IList<Card> ?? cardsPlayed.ToList();

            if (ShouldBurn(gameState.PlayPileStack))
                return;
            
            var ruleForPlayersCard = this.GetRuleForCardFromCardValue(cardsPlayed.First().Value);
            this.SetPlayerFromOrderOfPlay(gameState);

            if (ruleForPlayersCard != RuleForCard.SkipPlayer)
                return;

            var skipCardValue = this.RulesForCardsByValue.First(w => w.Value == RuleForCard.SkipPlayer).Key;
            var topCardsInPlayPileWithSkipValue = cardsPlayed.GetTopCardsWithSameValue(skipCardValue);

            foreach (var card in topCardsInPlayPileWithSkipValue)
                this.SetPlayerFromOrderOfPlay(gameState);
        }

        private bool ShouldBurn(IEnumerable<Card> cardsToCheck)
        {
            cardsToCheck = cardsToCheck as IList<Card> ?? cardsToCheck.ToList();
            if(!cardsToCheck.Any()) return false;
            
            var lastFourCardsAreSameValue = cardsToCheck.GetTopCardsWithSameValue(cardsToCheck.First().Value).Count() >= 4;
            var isBurnCard = this.GetRuleForCardFromCardValue(cardsToCheck.First().Value) == RuleForCard.Burn;

            return isBurnCard || lastFourCardsAreSameValue;
        }

        private void SetPlayerFromOrderOfPlay(GameState state)
        {
            if (state.OrderOfPlay == OrderOfPlay.Forward)
                state.CurrentPlayerLinkedListNode = state.CurrentPlayerLinkedListNode.Next ?? state.Players.First;
            else
                state.CurrentPlayerLinkedListNode = state.CurrentPlayerLinkedListNode.Previous ?? state.Players.Last;
        }

        private RuleForCard GetRuleForCardFromCardValue(CardValue cardValue)
        {
            RuleForCard ruleForCard;
            this.RulesForCardsByValue.TryGetValue(cardValue, out ruleForCard);
            return ruleForCard == 0 ? RuleForCard.Standard : ruleForCard;
        }
    }
}