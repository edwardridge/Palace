using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palace
{
    public static class EnumerableCardExtensions
    {
        public static IEnumerable<Card> GetTopCardsWithSameValue(this IEnumerable<Card> cards, CardValue cardValue)
        {
            List<Card> cardsToReturn = new List<Card>();
            foreach (var cardToCheck in cards)
            {
                if (cardToCheck.Value == cardValue)
                    cardsToReturn.Add(cardToCheck);
                else
                {
                    break;
                }
            }

            return cardsToReturn;
        } 
    }
}
