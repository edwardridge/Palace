namespace Palace
{
    using System.Collections.Generic;

    public class RulesProcessesor
    {
        private readonly Dictionary<CardValue, RuleForCard> _rulesForCardsByValue;

        public RulesProcessesor(Dictionary<CardValue, RuleForCard> rulesForCardsByValue)
        {
            this._rulesForCardsByValue = rulesForCardsByValue;
        }

        internal bool CardCanBePlayed(Card cardToPlay, Card lastCardPlayed)
        {
            if (lastCardPlayed == null)
                return true;

            var rulesForPlayersCard = this.getRuleForCardFromCardValue(cardToPlay.Value);
            var ruleForLastCardPlayed = this.getRuleForCardFromCardValue(lastCardPlayed.Value);

            if (rulesForPlayersCard == RuleForCard.Reset || rulesForPlayersCard == RuleForCard.Burn)
                return true;
            if (ruleForLastCardPlayed == RuleForCard.Standard && cardToPlay.Value < lastCardPlayed.Value)
                return false;
            if (ruleForLastCardPlayed == RuleForCard.LowerThan && cardToPlay.Value > lastCardPlayed.Value)
                return false;

            return true;
        }

        internal OrderOfPlay ChooseOrderOfPlay(OrderOfPlay currentOrder, Card cardToPlay)
        {
            var rulesForPlayersCard = this.getRuleForCardFromCardValue(cardToPlay.Value);
            if (rulesForPlayersCard == RuleForCard.ReverseOrderOfPlay)
                return currentOrder == OrderOfPlay.Forward ? OrderOfPlay.Backward : OrderOfPlay.Forward;

            return currentOrder;
        }

        internal bool PlayPileShouldBeCleared(Card cardToPlay)
        {
            return getRuleForCardFromCardValue(cardToPlay.Value) == RuleForCard.Burn;
        }

        internal LinkedListNode<Player> ChooseNextPlayer(
            Card cardToPlay, 
            LinkedList<Player> players, 
            LinkedListNode<Player> currentPlayer, 
            OrderOfPlay orderOfPlay)
        {
            var ruleForPlayersCard = getRuleForCardFromCardValue(cardToPlay.Value);
            if (ruleForPlayersCard == RuleForCard.Burn)
                return currentPlayer;

            if (orderOfPlay == OrderOfPlay.Forward)
                return currentPlayer.Next ?? players.First;
            
            return currentPlayer.Previous ?? players.Last;
        }

        private RuleForCard getRuleForCardFromCardValue(CardValue cardValue)
        {
            RuleForCard ruleForCard;
            _rulesForCardsByValue.TryGetValue(cardValue, out ruleForCard);
            return ruleForCard == 0 ? RuleForCard.Standard : ruleForCard;
        }
    }
}