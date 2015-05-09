namespace Palace_Console
{
    using System.Collections.Generic;

    using Palace;

    class Program
    {
        static void Main(string[] args)
        {
            var ruleForCardsByValue = new Dictionary<CardValue, RuleForCard>();
            ruleForCardsByValue.Add(CardValue.Seven, RuleForCard.LowerThan);
            ruleForCardsByValue.Add(CardValue.Ten, RuleForCard.Burn);
            ruleForCardsByValue.Add(CardValue.Two, RuleForCard.Reset);
            ruleForCardsByValue.Add(CardValue.Eight, RuleForCard.SeeThrough);
            ruleForCardsByValue.Add(CardValue.Jack, RuleForCard.SkipPlayer);
            ruleForCardsByValue.Add(CardValue.Ace, RuleForCard.ReverseOrderOfPlay);
        }
    }
}
