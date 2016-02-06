using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palace.Rules
{
    public class RulesForGame
    {
        public RulesForGame()
        {
            this.RuleList = new List<Rule>();
        }

        public void Add(Rule rule)
        {
            this.RuleList.Add(rule);
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
            return rule == null? new CardValue?() : rule.CardValue;
        }

        public ICollection<Rule> RuleList { get; private set; }
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
}
