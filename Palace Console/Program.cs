namespace Palace_Console
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    using Palace;
    using Palace.Repository;
    using Palace.Rules;

    class Program
    {
        private static Game game;
        static void Main(string[] args)
        {
            var rules = InitialiseRules();
            var players = InitialisePlayers();
            var dealer = new Dealer(players, new StandardDeck(), new DefaultStartGameRules(), rules);
            dealer.DealIntialCards();

            var output = "Press \"e\" to exit at any point\n";
            foreach (var player in players)
            {
                output += PrintPlayersCards(player);
            }

            int currentPlayerIndex = 0;
            var currentPlayer = players.ToArray()[currentPlayerIndex];
            output += currentPlayer.Name + ": Put your cards face up. Press the number to put that card face down. When you're done press r.";
            Console.WriteLine(output);

            while (true)
            {
                if (StopSetupPhase(players, ref currentPlayer, ref currentPlayerIndex))
                    break;
            }


            game = dealer.StartGame();
            currentPlayer = game.State.CurrentPlayer;

            Console.WriteLine("It's " + currentPlayer.Name + " turn");
            Console.WriteLine(PrintPlayersCards(currentPlayer));
            
            while (true)
            {
                var lastCardAsString = game.State.PlayPile.First() == null ? "None" : game.State.PlayPile.First().Value + " of " + game.State.PlayPile.First().Suit;
                Console.WriteLine("Last card played: " + lastCardAsString);
                var line = Console.ReadLine();
                if (line.Equals("e"))
                    break;

                if (line.Equals("c"))
                {
                    game.PlayerCannotPlayCards(currentPlayer);
                }
                else if (line.Equals("s"))
                {
                    var gameRepository = new GameRepository(new PalaceDocumentSession());
                    gameRepository.Save(game);
                }
                
                else
                {
                    try
                    {
                        var indexes = line.Split(',').Select(int.Parse);
                        var cardToPlay = currentPlayer.CardsInHand.ToArray().Where((w, i) => indexes.Contains(i));
                        game.PlayInHandCards(currentPlayer, cardToPlay.ToList());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error reading input, please try again.");
                    }
                }

                currentPlayer = game.State.CurrentPlayer;
                Console.WriteLine("It's " + currentPlayer.Name + " turn");
                Console.WriteLine(PrintPlayersCards(currentPlayer));
            }
            
            
        }

        private static bool StopSetupPhase(Player[] players, ref Player currentPlayer, ref int currentPlayerIndex)
        {
            var line = Console.ReadLine();
            line = line ?? "";
            if (line.Equals("e"))
                return true;

            if (line.Equals("r"))
            {
                if (currentPlayer.CardsFaceUp.Count != 3)
                    Console.WriteLine("You must put 3 cards up");
                else
                {
                    currentPlayerIndex++;
                    if (currentPlayerIndex < players.Count())
                    {
                        currentPlayer = players.ToArray()[currentPlayerIndex];
                        Console.WriteLine(currentPlayer.Name + ": Put your cards face up. Press the number to put that card face down. When you're done press r.");
                        Console.WriteLine(PrintPlayersCards(currentPlayer));
                    }
                    else
                        return true;
                }
            }
            else if (line.Contains("o"))
            {
                var lineItems = line.Split(',');
                var gameRepository = new GameRepository(new PalaceDocumentSession());
                game = gameRepository.Open(lineItems[1]);
                return true;
            }
            else
            {
                try
                {
                    var indexes = line.Split(',').Select(int.Parse);
                    var cardsToPutFaceUp = currentPlayer.CardsInHand.ToArray().Where((w, i) => indexes.Contains(i));
                   // foreach (var card in cardsToPutFaceUp)
                     //   currentPlayer.PutCardFaceUp(card);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error reading input, please try again.");
                }
                
                Console.WriteLine(PrintPlayersCards(currentPlayer));
            }
            return false;
        }

        private static string PrintPlayersCards(Player player)
        {
            string output = "";
            output += player.Name + "\n";
            output += "Cards in hand: \n";
            for (var i = 0; i < player.CardsInHand.Count(); i++)
            {
                output += i + ": " + player.CardsInHand.ToArray()[i].Value + " of " + player.CardsInHand.ToArray()[i].Suit + "\n";
            }

            output += "Cards face up: \n";
            for (var i = 0; i < player.CardsFaceUp.Count(); i++)
            {
                output += i + ": " + player.CardsFaceUp.ToArray()[i].Value + " of " + player.CardsFaceUp.ToArray()[i].Suit + "\n";
            }

            output += "\n";
            return output;
        }

        private static Player[] InitialisePlayers()
        {
            var player1 = new Player("Ed");
            var player2 = new Player("Soph");
            var players = new[] { player1, player2 };
            return players;
        }

        private static Dictionary<CardValue, RuleForCard> InitialiseRules()
        {
            var ruleForCardsByValue = new Dictionary<CardValue, RuleForCard>();
            ruleForCardsByValue.Add(CardValue.Seven, RuleForCard.LowerThan);
            ruleForCardsByValue.Add(CardValue.Ten, RuleForCard.Burn);
            ruleForCardsByValue.Add(CardValue.Two, RuleForCard.Reset);
            ruleForCardsByValue.Add(CardValue.Eight, RuleForCard.SeeThrough);
            ruleForCardsByValue.Add(CardValue.Jack, RuleForCard.SkipPlayer);
            ruleForCardsByValue.Add(CardValue.Ace, RuleForCard.ReverseOrderOfPlay);
            return ruleForCardsByValue;
        }
    }
}
