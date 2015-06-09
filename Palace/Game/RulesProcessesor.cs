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

        internal bool PlayPileShouldBeCleared(IEnumerable<Card> cardsToPlay, IEnumerable<Card> playPile)
        {
            var cardsToCheck = cardsToPlay.ToList();
            var lastFourCardsInPlayPile = playPile.Reverse().Take(4);
            foreach (var card in lastFourCardsInPlayPile)
            {
                if (card.Value == cardsToCheck.First().Value)
                    cardsToCheck.Add(card);
                else
                    break;
            }
            var fourOfAKind = cardsToCheck.Count() >= 4;
            var isBurnCard = getRuleForCardFromCardValue(cardsToCheck.First().Value) == RuleForCard.Burn;

            return fourOfAKind || isBurnCard;
        }

        internal LinkedListNode<Player> ChooseNextPlayer(
            IEnumerable<Card> cards, 
            LinkedList<Player> players, 
            LinkedListNode<Player> currentPlayer, 
            OrderOfPlay orderOfPlay)
        {
            if (cards == null)
                return ChoosePlayerFromOrderOfPlay(orderOfPlay, players, currentPlayer);

            var cardsList = cards as IList<Card> ?? cards.ToList();
            var cardToPlay = cardsList.First();
            var ruleForPlayersCard = getRuleForCardFromCardValue(cardToPlay.Value);
            if (ruleForPlayersCard == RuleForCard.Burn || cardsList.Count >= 4)
                return currentPlayer;

            var nextPlayer = ChoosePlayerFromOrderOfPlay(orderOfPlay, players, currentPlayer);
            if (ruleForPlayersCard != RuleForCard.SkipPlayer)
                return nextPlayer;

            foreach (var card in cardsList)
                nextPlayer = ChoosePlayerFromOrderOfPlay(orderOfPlay, players, nextPlayer);
            
            return nextPlayer;
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