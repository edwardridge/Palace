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
            var lastCardPlayed = playPileAsList.Any() ? playPileAsList.First() : null;
            if (lastCardPlayed == null)
                return true;

            var rulesForPlayersCard = this.getRuleForCardFromCardValue(cardToPlay.Value);
            var ruleForLastCardPlayed = this.getRuleForCardFromCardValue(lastCardPlayed.Value);

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

        internal OrderOfPlay ChooseOrderOfPlay(OrderOfPlay currentOrder, Card cardToPlay)
        {
            var rulesForPlayersCard = this.getRuleForCardFromCardValue(cardToPlay.Value);
            if (rulesForPlayersCard == RuleForCard.ReverseOrderOfPlay)
                return currentOrder == OrderOfPlay.Forward ? OrderOfPlay.Backward : OrderOfPlay.Forward;

            return currentOrder;
        }

        internal bool PlayPileShouldBeCleared(IEnumerable<Card> playPile)
        {
            return this.ShouldBurn(playPile);
        }

        internal LinkedListNode<Player> ChooseNextPlayer(
            IEnumerable<Card> playPile, 
            LinkedList<Player> players, 
            LinkedListNode<Player> currentPlayer, 
            OrderOfPlay orderOfPlay)
        {
            var cardsList = playPile as IList<Card> ?? playPile.ToList();

            if (!cardsList.Any())
                return ChoosePlayerFromOrderOfPlay(orderOfPlay, players, currentPlayer);

            if (ShouldBurn(cardsList))
                return currentPlayer;

            var ruleForPlayersCard = getRuleForCardFromCardValue(cardsList.First().Value);
            var nextPlayer = ChoosePlayerFromOrderOfPlay(orderOfPlay, players, currentPlayer);
            if (ruleForPlayersCard != RuleForCard.SkipPlayer)
                return nextPlayer;

            foreach (var card in cardsList)
                nextPlayer = ChoosePlayerFromOrderOfPlay(orderOfPlay, players, nextPlayer);
            
            return nextPlayer;
        }

        private bool ShouldBurn(IEnumerable<Card> cardsToCheck)
        {
            var cardsToCheckAsList = cardsToCheck as IList<Card> ?? cardsToCheck.ToList();
            var lastFourCardsAreSameValue = cardsToCheckAsList.Count() >= 4 && cardsToCheckAsList.Take(4).Select(s => s.Value).Distinct().Count() == 1;

            var isBurnCard = this.getRuleForCardFromCardValue(cardsToCheckAsList.First().Value) == RuleForCard.Burn;
            return isBurnCard || lastFourCardsAreSameValue;
        }

        private LinkedListNode<Player> ChoosePlayerFromOrderOfPlay(OrderOfPlay orderOfPlay, LinkedList<Player> players, LinkedListNode<Player> currentPlayer)
        {
            if (orderOfPlay == OrderOfPlay.Forward)
                return currentPlayer.Next ?? players.First;
            
            return currentPlayer.Previous ?? players.Last;
        }

        private RuleForCard getRuleForCardFromCardValue(CardValue cardValue)
        {
            RuleForCard ruleForCard;
            this.RulesForCardsByValue.TryGetValue(cardValue, out ruleForCard);
            return ruleForCard == 0 ? RuleForCard.Standard : ruleForCard;
        }
    }
}