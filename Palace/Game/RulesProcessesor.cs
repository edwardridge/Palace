using System.Collections.Generic;

namespace Palace
{
    using System.Linq;

    public class RulesProcessesor
    {
        private readonly Dictionary<CardValue, RuleForCard> _rulesForCardsByValue;

        public RulesProcessesor(Dictionary<CardValue, RuleForCard> rulesForCardsByValue)
        {
            this._rulesForCardsByValue = rulesForCardsByValue;
        }

        public bool ProcessRulesForGame(IReverseOrderOfPlay order, Card cardToPlay, Card lastCardPlayed)
        {
            
            if (lastCardPlayed == null)
                return true;

            var rulesForPlayersCard = this.getRuleForCardFromCardValue(cardToPlay.Value);
            var ruleForLastCardPlayed = this.getRuleForCardFromCardValue(lastCardPlayed.Value);

            if (ruleForLastCardPlayed == RuleForCard.Reset || rulesForPlayersCard == RuleForCard.Reset)
                return true;
            if (ruleForLastCardPlayed == RuleForCard.Standard && cardToPlay.Value < lastCardPlayed.Value)
                return false;
            if (ruleForLastCardPlayed == RuleForCard.LowerThan && cardToPlay.Value > lastCardPlayed.Value)
                return false;

            return true;
        }

        public bool ChooseOrderOfPlay(bool currentOrder, Card cardToPlay)
        {
            var rulesForPlayersCard = this.getRuleForCardFromCardValue(cardToPlay.Value);
            if (rulesForPlayersCard == RuleForCard.ReverseOrderOfPlay)
                return !currentOrder;

            return currentOrder;
        }

        private RuleForCard getRuleForCardFromCardValue(CardValue cardValue)
        {
            RuleForCard ruleForCard;
            _rulesForCardsByValue.TryGetValue(cardValue, out ruleForCard);
            return ruleForCard == 0 ? RuleForCard.Standard : ruleForCard;
        }
    }
}
