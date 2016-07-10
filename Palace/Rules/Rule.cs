using System;
using System.Collections.Generic;
using System.Linq;

namespace Palace.Rules
{
    public class RulesForGame
    {
        public RulesForGame()
        {
            this.RuleList = new List<Rule>();
            this.ListOfIRules = new List<IRule>();
        }

        public void Add(Rule rule)
        {
            this.RuleList.Add(rule);
        }

        public void AddIRule(IRule rule)
        {
            this.ListOfIRules.Add(rule);
        }

        public RulesForGame(IEnumerable<Rule> rules)
        {
            this.RuleList = new List<Rule>(rules);
        }

        //public RuleForCard GetRuleFromCard(CardValue cardValue)
        //{
        //    var rule = RuleList.FirstOrDefault(f => f.CardValue == cardValue);
        //    return rule == null ? RuleForCard.Standard : rule.RuleForCard;
        //}

        public CardValue? GetCardValueFromRule(RuleForCard ruleFromCard)
        {
            var rule = RuleList.FirstOrDefault(f => f.RuleForCard == ruleFromCard);
            return rule == null ? new CardValue?() : rule.CardValue;
        }

        public CardValue? GetResetCardValue()
        {
            return this.ListOfIRules.FirstOrDefault(f => f.GetType() == typeof(ResetRule))?.CardValue;
        }

        public ICollection<Rule> RuleList { get; private set; }

        //TODO: rename from this abomination
        public ICollection<IRule> ListOfIRules { get; private set; }
    }

    public class Rule
    {
        public Rule(CardValue cardValue, RuleForCard ruleForCard)
        {
            this.CardValue = cardValue;
            this.RuleForCard = ruleForCard;
        }

        public CardValue CardValue { get; private set; }

        public RuleForCard RuleForCard { get; private set; }
    }

    public interface IRule
    {
        void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed);

        CardValue CardValue { get; }

        bool CanBePlayed(CardValue previousCardValue);

        bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed);

        bool IsPowerRule { get; }

        bool AffectsNextCard { get; }
    }

    public class RuleBase
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

        protected void SetNextPlayer(GameState gamestate)
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
            //var isBurnCard = this.GetRuleForCardFromCardValue(cardsToCheck.First().Value) == RuleForCard.Burn;

            //return isBurnCard || lastFourCardsAreSameValue;
            return lastFourCardsAreSameValue;
        }

        protected bool IsCardValueHigherThanCardPlayed(CardValue previousCardValue)
        {
            return this.CardValue >= previousCardValue;
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
    }

    public class BurnRule : RuleBase, IRule
    {
        public BurnRule(CardValue cardValue) : base(cardValue)
        {
           
        }

        public bool AffectsNextCard
        {
            get
            {
                return false;
            }
        }

        public bool IsPowerRule
        {
            get
            {
                return true;
            }
        }

        public bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed)
        {
            return true;
        }

        public void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed)
        {
            gamestate.PlayPileStack.Clear(); 
        }

        public bool CanBePlayed(CardValue previousCardValue)
        {
            return true;
        }


    }

    public class ResetRule : RuleBase, IRule
    {
        public ResetRule(CardValue cardValue) : base(cardValue)
        {

        }

        public bool AffectsNextCard
        {
            get
            {
                return false;
            }
        }

        public bool IsPowerRule
        {
            get
            {
                return true;
            }
        }

        public bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed)
        {
            return true;
        }

        public void Apply(GameState gameState, IEnumerable<Card> cardsPlayed)
        {
            this.SetNextPlayer(gameState);
        }
        
        public bool CanBePlayed(CardValue previousCardValue)
        {
            return true;
        }
    }

    public class SkipRule : RuleBase, IRule
    {
        public SkipRule(CardValue cardValue) : base(cardValue)
        {

        }

        public bool AffectsNextCard
        {
            get
            {
                return false;
            }
        }

        public bool IsPowerRule
        {
            get
            {
                return false;
            }
        }

        public bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed)
        {
            return true;
        }

        public void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed)
        {
            this.SetNextPlayer(gamestate);
            var topCardsInPlayPileWithSkipValue = cardsPlayed.GetTopCardsWithSameValue(this.CardValue);

            foreach (var card in topCardsInPlayPileWithSkipValue)
                this.SetNextPlayer(gamestate);
        }

        public bool CanBePlayed(CardValue previousCardValue)
        {
            return IsCardValueHigherThanCardPlayed(previousCardValue);
        }
    }

    public class ReverseOrderOfPlayRule : RuleBase, IRule
    {
        public ReverseOrderOfPlayRule(CardValue cardValue) : base(cardValue)
        {

        }

        public bool AffectsNextCard
        {
            get
            {
                return false;
            }
        }

        public bool IsPowerRule
        {
            get
            {
                return false;
            }
        }

        public bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed)
        {
            return true;
        }

        public void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed)
        {
            gamestate.OrderOfPlay = gamestate.OrderOfPlay == OrderOfPlay.Forward ? OrderOfPlay.Backward : OrderOfPlay.Forward;
            this.SetNextPlayer(gamestate);
        }

        public bool CanBePlayed(CardValue previousCardValue)
        {
            return IsCardValueHigherThanCardPlayed(previousCardValue);
        }
    }

    public class SeeThroughRule : RuleBase, IRule
    {
        public SeeThroughRule(CardValue cardValue) : base(cardValue)
        {

        }

        public bool AffectsNextCard
        {
            get
            {
                return true;
            }
        }

        public bool IsPowerRule
        {
            get
            {
                return true;
            }
        }

        public bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed)
        {
            var lastCardNotSeeThrough = gameState.PlayPile.Except(gameState.PlayPile.GetTopCardsWithSameValue(this.CardValue)).First();
            return cardToBePlayed >= lastCardNotSeeThrough.Value;
        }

        public void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed)
        {
            this.SetNextPlayer(gamestate);
        }

        public bool CanBePlayed(CardValue previousCardValue)
        {
            return true;
        }
    }

    public class LowerThanRule : RuleBase, IRule
    {
        public LowerThanRule(CardValue cardValue) : base(cardValue)
        {

        }

        public bool AffectsNextCard
        {
            get
            {
                return true;
            }
        }

        public bool IsPowerRule
        {
            get
            {
                return false;
            }
        }

        public bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed)
        {
            return cardToBePlayed <= this.CardValue;
        }

        public void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed)
        {
            this.SetNextPlayer(gamestate);
        }

        public bool CanBePlayed(CardValue previousCardValue)
        {
            return this.IsCardValueHigherThanCardPlayed(previousCardValue);
        }
    }

    public class StandardRule : RuleBase, IRule
    {
        public StandardRule(CardValue cardValue) : base(cardValue)
        {

        }

        public bool AffectsNextCard
        {
            get
            {
                return false;
            }
        }

        public bool IsPowerRule
        {
            get
            {
                return false;
            }
        }

        public bool AllowsNextCard(GameState gameState, CardValue cardToBePlayed)
        {
            return true;
        }

        public void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed)
        {
            this.SetNextPlayer(gamestate);
        }

        public bool CanBePlayed(CardValue previousCardValue)
        {
            return this.IsCardValueHigherThanCardPlayed(previousCardValue);
        }
    }
}
