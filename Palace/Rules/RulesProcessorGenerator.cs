namespace Palace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RulesProcessorGenerator
    {

        private Guid gameId;

        private Dictionary<CardValue, RuleForCard> _rulesForCardsByValue;

        public RulesProcessorGenerator(Guid gameId, Dictionary<CardValue, RuleForCard> rulesForCardsByValue)
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

        internal RuleProcessor GetRuleProcessor(GameState state, IEnumerable<Card> cardsPlayed)
        {
            return new RuleProcessor(this.RulesForCardsByValue, state, cardsPlayed);
        }
    }

    //Todo: Rename!
    internal class RuleProcessor
    {
        private readonly Dictionary<CardValue, RuleForCard> _rulesForCardsByValue;

        private readonly GameState state;

        private readonly IEnumerable<Card> cardsPlayed;

        public RuleProcessor(Dictionary<CardValue, RuleForCard> rulesForCardsByValue, GameState state, IEnumerable<Card> cardsPlayed)
        {
            this._rulesForCardsByValue = rulesForCardsByValue;
            this.state = state;
            this.cardsPlayed = cardsPlayed;
        }

        internal GameState GetNextState(PlayerCardType playerCardType)
        {

            this.RemoveCardsFromPlayer(state.CurrentPlayer, this.cardsPlayed.ToList(), playerCardType);

            foreach (Card card in this.cardsPlayed)
                state.PlayPileStack.Push(card);

            state.OrderOfPlay = this.GetOrderOfPlay();

            if (!state.CurrentPlayer.HasNoMoreCards())
            {
                state.CurrentPlayer = this.SetNextPlayer();

                if (this.PlayPileShouldBeCleared())
                    state.PlayPileStack.Clear();
            }
            else
                state.GameOver = true;
                
            return state;
        }

        internal GameState GetNextStateWhenCardCannotBePlayed(Player player)
        {
            player.AddCardsToInHandPile(state.PlayPileStack);
            state.PlayPileStack.Clear();
            state.CurrentPlayer = this.SetNextPlayer();
            return state;
        }

        internal bool CardCanBePlayed()
        {
            return this.CheckCardCanBePlayed(cardsPlayed.First(), state.PlayPile);
        }

        private Player SetNextPlayer()
        {
            if (cardsPlayed == null)
            {
                var nextPlayer = this.GetNextPlayerFromOrderOfPlay(state.OrderOfPlay, state.CurrentPlayerLinkedListNode);
                return nextPlayer.Value;
            }

            if (ShouldBurn(state.PlayPileStack))
                return state.CurrentPlayerLinkedListNode.Value;

            var ruleForPlayersCard = this.GetRuleForCardFromCardValue(cardsPlayed.First().Value);
            var nextPayer = this.GetNextPlayerFromOrderOfPlay(state.OrderOfPlay, state.CurrentPlayerLinkedListNode);

            if (ruleForPlayersCard != RuleForCard.SkipPlayer)
                return nextPayer.Value;

            var skipCardValue = this._rulesForCardsByValue.First(w => w.Value == RuleForCard.SkipPlayer).Key;
            var topCardsInPlayPileWithSkipValue = cardsPlayed.GetTopCardsWithSameValue(skipCardValue);

            foreach (var card in topCardsInPlayPileWithSkipValue)
                nextPayer = this.GetNextPlayerFromOrderOfPlay(state.OrderOfPlay, nextPayer);

            return nextPayer.Value;
        }

        private void RemoveCardsFromPlayer(Player player, ICollection<Card> cards, PlayerCardType playerCardType)
        {
            if (playerCardType == PlayerCardType.InHand)
            {
                player.RemoveCardsFromInHand(cards);

                while (player.CardsInHand.Count < 3 && state.Deck.CardsRemaining)
                {
                    player.AddCardsToInHandPile(state.Deck.DealCards(1));
                }
            }
            if (playerCardType == PlayerCardType.FaceDown)
                player.RemoveCardsFromFaceDown(cards);
            if (playerCardType == PlayerCardType.FaceUp)
                player.RemoveCardsFromFaceUp(cards);
        }
        
        private bool PlayPileShouldBeCleared()
        {
            return this.ShouldBurn(state.PlayPile);
        }

        private OrderOfPlay GetOrderOfPlay()
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