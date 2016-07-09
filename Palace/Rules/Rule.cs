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

        public RuleForCard GetRuleFromCard(CardValue cardValue)
        {
            var rule = RuleList.FirstOrDefault(f => f.CardValue == cardValue);
            return rule == null ? RuleForCard.Standard : rule.RuleForCard;
        }

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

        public void SetNextPlayer(GameState gamestate)
        {
            gamestate.CurrentPlayerName = GetNextPlayerFromOrderOfPlay(gamestate);
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

        public void Apply(GameState gamestate, IEnumerable<Card> cardsPlayed)
        {
            this.SetNextPlayer(gamestate);
            var topCardsInPlayPileWithSkipValue = cardsPlayed.GetTopCardsWithSameValue(this.CardValue);

            foreach (var card in topCardsInPlayPileWithSkipValue)
                this.SetNextPlayer(gamestate);
        }

        public bool CanBePlayed(CardValue previousCardValue)
        {
            return this.CardValue >= previousCardValue;
        }
    }
}
