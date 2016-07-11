namespace Palace
{
    using Rules;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RulesProcessorGenerator
    {
        private Guid gameId;

        private RulesForGame _rulesForCardsByValue;

        public RulesProcessorGenerator(Guid gameId, RulesForGame rules)
        {
            this.GameId = gameId;
            this.RulesForCardsByValue = rules;
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

        internal RulesForGame RulesForCardsByValue
        {
            get
            {
                return _rulesForCardsByValue;
            }
            set
            {
                _rulesForCardsByValue = value;
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
        private readonly RulesForGame RulesForGame;

        private readonly GameState State;

        private readonly IEnumerable<Card> CardsPlayed;

        public RuleProcessor(RulesForGame rulesForGame, GameState state, IEnumerable<Card> cardsPlayed)
        {
            this.RulesForGame = rulesForGame;
            this.State = state;
            this.CardsPlayed = cardsPlayed;
        }

        internal GameState GetNextState(PlayerCardType playerCardType)
        {
            this.RemoveCardsFromPlayer(State.CurrentPlayer, this.CardsPlayed.ToList(), playerCardType);

            foreach (Card card in this.CardsPlayed)
                State.PlayPileStack.Push(card);

            if (State.CurrentPlayer.HasNoMoreCards())
            {
                State.GameOver = true;
            }
            else
            {
                var ruleToApply = this.RulesForGame.GetRule(this.CardsPlayed.First().Value);
                ruleToApply.Apply(State, this.CardsPlayed);
            }

            State.NumberOfValdMoves += 1;

            return State;
        }

        internal GameState GetNextStateWhenPlayingFaceDownCard()
        {
            var player = State.Players.First(f => f.Name == State.CurrentPlayerName);
            State.CurrentPlayerName = GetNextPlayerName();
            player.AddCardsToInHandPile(State.PlayPile);
            player.AddCardsToInHandPile(CardsPlayed);
            player.RemoveCardsFromFaceDown(CardsPlayed.ToList());
            State.PlayPileStack.Clear();
            State.NumberOfValdMoves += 1;

            return State;
        }

        internal GameState GetNextStateWhenCardCannotBePlayed(Player player)
        {
            player.AddCardsToInHandPile(State.PlayPileStack);
            State.PlayPileStack.Clear();
            State.CurrentPlayerName = this.GetNextPlayerName();
            State.NumberOfValdMoves += 1;
            return State;
        }

        internal bool CardCanBePlayed()
        {
            var playPile = State.PlayPile.ToList();
            var cardToPlay = this.CardsPlayed.First();
            if (!playPile.Any())
                return true;

            var lastCardPlayed = playPile.First();

            var ruleToApply = this.RulesForGame.GetRule(this.CardsPlayed.First().Value);

            if (ruleToApply.CanAlwaysPlayCard)
            {
                return true;
            }

            var previousRuleToApply = this.RulesForGame.GetRule(lastCardPlayed.Value);

            if (previousRuleToApply.AffectsNextCard)
            {
                return previousRuleToApply.AllowsNextCard(State, cardToPlay.Value);
            }

            return IsCardValueHigherThanCardPlayed(cardToPlay.Value, lastCardPlayed.Value);
        }

        private string GetNextPlayerName()
        {
            if (CardsPlayed == null)
            {
                var nextPlayer = this.GetNextPlayerFromOrderOfPlay(State.OrderOfPlay, State.CurrentPlayerLinkedListNode);
                return nextPlayer.Value.Name;
            }

            if (ShouldBurn(State.PlayPileStack))
                return State.CurrentPlayerLinkedListNode.Value.Name;
            
            var nextPayer = this.GetNextPlayerFromOrderOfPlay(State.OrderOfPlay, State.CurrentPlayerLinkedListNode);

            return nextPayer.Value.Name;
        }

        //Todo: This removes cards from deck, should be explicit
        private void RemoveCardsFromPlayer(Player player, ICollection<Card> cards, PlayerCardType playerCardType)
        {
            if (playerCardType == PlayerCardType.InHand)
            {
                player.RemoveCardsFromInHand(cards);

                while (player.CardsInHand.Count < 3 && State.Deck.CardsRemaining)
                {
                    player.AddCardsToInHandPile(State.Deck.DealCards(1));
                }
            }
            if (playerCardType == PlayerCardType.FaceDown)
                player.RemoveCardsFromFaceDown(cards);
            if (playerCardType == PlayerCardType.FaceUp)
                player.RemoveCardsFromFaceUp(cards);
        }

        private LinkedListNode<Player> GetNextPlayerFromOrderOfPlay(OrderOfPlay orderOfPlay, LinkedListNode<Player> player)
        {
            if (orderOfPlay == OrderOfPlay.Forward)
                return player.Next ?? State.Players.First;
            
            return player.Previous ?? State.Players.Last;
        }

        private bool ShouldBurn(IEnumerable<Card> cardsToCheck)
        {
            cardsToCheck = cardsToCheck as IList<Card> ?? cardsToCheck.ToList();
            if (!cardsToCheck.Any()) return false;

            var lastFourCardsAreSameValue = cardsToCheck.GetTopCardsWithSameValue(cardsToCheck.First().Value).Count() >= 4;
            return lastFourCardsAreSameValue;
        }

        protected bool IsCardValueHigherThanCardPlayed(CardValue currentCardValue, CardValue previousCardValue)
        {
            return currentCardValue >= previousCardValue;
        }
    }
}