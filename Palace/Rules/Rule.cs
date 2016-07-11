using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace.Rules
{
    public class RulesForGame
    {
        private ICollection<IRule> _ruleList;

        public RulesForGame()
        {
            this._ruleList = new List<IRule>();
        }

        public void AddRule(IRule rule)
        {
            this._ruleList.Add(rule);
        }

        public IRule GetRule(CardValue cardValue)
        {
            var ruleToApply = this._ruleList.FirstOrDefault(f => f.CardValue == cardValue) ?? new StandardRule(cardValue);
            return ruleToApply;
        }

        public CardValue? GetResetCardValue()
        {
            return this._ruleList.FirstOrDefault(f => f.GetType() == typeof(ResetRule))?.CardValue;
        }
        
        public ICollection<IRule> Rules { get { return _ruleList; }  }
    }
    
    public interface IRule
    {
        void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed);

        CardValue CardValue { get; }

        bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed);

        bool CanAlwaysPlayCard { get; }

        bool AffectsNextCard { get; }

        string Description { get; }
    }

    public abstract class RuleBase : IRule
    {
        private readonly CardValue cardValue;

        public RuleBase(CardValue cardValue)
        {
            this.cardValue = cardValue;
        }
        public CardValue CardValue
        {
            get
            {
                return cardValue;
            }
        }

        public abstract bool CanAlwaysPlayCard
        {
            get;
        }

        public abstract bool AffectsNextCard
        {
            get;
        }

        public string Description { get { return this.GetType().ToString(); } }

        protected void CheckIfPlayPileShoudBeClearedAndSetNextPlayer(GameState gamestate)
        {
            if (this.ShouldBurn(gamestate.PlayPile))
            {
                gamestate.PlayPileStack.Clear();
            }
            else
            {
                gamestate.CurrentPlayerName = GetNextPlayerFromOrderOfPlay(gamestate);
            }
        }

        private bool ShouldBurn(IEnumerable<Card> cardsToCheck)
        {
            cardsToCheck = cardsToCheck as IList<Card> ?? cardsToCheck.ToList();
            if (!cardsToCheck.Any()) return false;

            var lastFourCardsAreSameValue = cardsToCheck.GetTopCardsWithSameValue(cardsToCheck.First().Value).Count() >= 4;
            return lastFourCardsAreSameValue;
        }

        private string GetNextPlayerFromOrderOfPlay(GameState gamestate)
        {
            var nextPlayer = gamestate.CurrentPlayerLinkedListNode;
            if (gamestate.OrderOfPlay == OrderOfPlay.Forward)
                nextPlayer = nextPlayer.Next ?? gamestate.Players.First;
            else
            {
                nextPlayer = gamestate.CurrentPlayerLinkedListNode.Previous ?? gamestate.Players.Last;
            }
            return nextPlayer.Value.Name;
        }

        public abstract void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed);

        public abstract bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed);
    }

    public class BurnRule : RuleBase
    {
        public BurnRule(CardValue cardValue) : base(cardValue)
        {
           
        }

        public override bool AffectsNextCard
        {
            get
            {
                return false;
            }
        }

        public override bool CanAlwaysPlayCard
        {
            get
            {
                return true;
            }
        }

        public override bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed)
        {
            return true;
        }

        public override void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed)
        {
            gamestate.PlayPileStack.Clear(); 
        }
        
    }

    public class ResetRule : RuleBase
    {
        public ResetRule(CardValue cardValue) : base(cardValue)
        {

        }

        public override bool AffectsNextCard
        {
            get
            {
                return true;
            }
        }

        public override bool CanAlwaysPlayCard
        {
            get
            {
                return true;
            }
        }

        public override bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed)
        {
            return true;
        }

        public override void Apply(GameState gameState, IEnumerable<Card> cardsPlayed)
        {
            this.CheckIfPlayPileShoudBeClearedAndSetNextPlayer(gameState);
        }
    }

    public class SkipRule : RuleBase
    {
        public SkipRule(CardValue cardValue) : base(cardValue)
        {

        }

        public override bool AffectsNextCard
        {
            get
            {
                return false;
            }
        }

        public override bool CanAlwaysPlayCard
        {
            get
            {
                return false;
            }
        }

        public override bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed)
        {
            return true;
        }

        public override void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed)
        {
            this.CheckIfPlayPileShoudBeClearedAndSetNextPlayer(gamestate);
            var topCardsInPlayPileWithSkipValue = cardsPlayed.GetTopCardsWithSameValue(this.CardValue);

            foreach (var card in topCardsInPlayPileWithSkipValue)
                this.CheckIfPlayPileShoudBeClearedAndSetNextPlayer(gamestate);
        }

    }

    public class ReverseOrderOfPlayRule : RuleBase
    {
        public ReverseOrderOfPlayRule(CardValue cardValue) : base(cardValue)
        {

        }

        public override bool AffectsNextCard
        {
            get
            {
                return false;
            }
        }

        public override bool CanAlwaysPlayCard
        {
            get
            {
                return false;
            }
        }

        public override bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed)
        {
            return true;
        }

        public override void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed)
        {
            gamestate.OrderOfPlay = gamestate.OrderOfPlay == OrderOfPlay.Forward ? OrderOfPlay.Backward : OrderOfPlay.Forward;
            this.CheckIfPlayPileShoudBeClearedAndSetNextPlayer(gamestate);
        }
    }

    public class SeeThroughRule : RuleBase
    {
        public SeeThroughRule(CardValue cardValue) : base(cardValue)
        {

        }

        public override bool AffectsNextCard
        {
            get
            {
                return true;
            }
        }

        public override bool CanAlwaysPlayCard
        {
            get
            {
                return true;
            }
        }

        public override bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed)
        {
            var lastCardNotSeeThrough = gameState.PlayPile.Except(gameState.PlayPile.GetTopCardsWithSameValue(this.CardValue)).First();
            return cardToBePlayed >= lastCardNotSeeThrough.Value;
        }

        public override void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed)
        {
            this.CheckIfPlayPileShoudBeClearedAndSetNextPlayer(gamestate);
        }
    }

    public class LowerThanRule : RuleBase
    {
        public LowerThanRule(CardValue cardValue) : base(cardValue)
        {

        }

        public override bool AffectsNextCard
        {
            get
            {
                return true;
            }
        }

        public override bool CanAlwaysPlayCard
        {
            get
            {
                return false;
            }
        }

        public override bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed)
        {
            return cardToBePlayed <= this.CardValue;
        }

        public override void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed)
        {
            this.CheckIfPlayPileShoudBeClearedAndSetNextPlayer(gamestate);
        }
    }

    public class StandardRule : RuleBase
    {
        public StandardRule(CardValue cardValue) : base(cardValue)
        {

        }

        public override bool AffectsNextCard
        {
            get
            {
                return false;
            }
        }

        public override bool CanAlwaysPlayCard
        {
            get
            {
                return false;
            }
        }

        public override bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed)
        {
            return true;
        }

        public override void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed)
        {
            this.CheckIfPlayPileShoudBeClearedAndSetNextPlayer(gamestate);
        }
    }
}
