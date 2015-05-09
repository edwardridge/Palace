namespace Palace_Console
{
    using System;
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
            var player1 = new Player("Ed");
            var player2 = new Player("Soph");
            var players = new[] { player1, player2 };
            var dealer = new Dealer(players, new StandardDeck(), new DefaultStartGameRules());
            dealer.DealIntialCards();

            var output = "";
            foreach (var player in players)
            {
                output += player.Name + "\n";
                output += "Cards: ";
                foreach (var inHandCards in player.CardsInHand)
                    output += inHandCards.Value + " of " + inHandCards.Suit + "\n";
                output += "\n";
            }
            Console.WriteLine(output);
            var t = Console.ReadLine();
            
        }
    }
}
