namespace Palace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Dealer
    {
        public Dealer(Deck deck, ICanStartGame canStartGame)
            : this(deck, canStartGame, new Dictionary<CardValue, RuleForCard>())
        {
        }

        public Dealer(Deck deck, ICanStartGame canStartGame, Dictionary<CardValue, RuleForCard> rulesForCardsByValue)
        {
            this._deck = deck;
            this._rulesForCardsByValue = rulesForCardsByValue;
            this.canStartGame = canStartGame;
        }

        public void DealIntialCards(ICollection<Player> players)
        {
            foreach (var player in players)
            {
                player.AddCardToFaceDownPile(_deck.DealCards(3));
                player.AddCardsToInHandPile(_deck.DealCards(6));
            }
        }

        public Game StartGame(ICollection<Player> players, Player startingPlayer = null)
        {
            if (!this.canStartGame.GameIsReadyToStart(players))
                throw new InvalidOperationException("The game is not ready to start");

            var game = new Game(players, new RulesProcessesor(_rulesForCardsByValue), _deck);
            if (startingPlayer == null)
            {
                startingPlayer = players.First();

                foreach (var player in players)
                {
                    if (player.CardsInHand == null || player.NumCardsInHand == 0)
                        continue;
                    if (player.LowestCardInValue.Value < startingPlayer.LowestCardInValue.Value)
                        startingPlayer = player;
                }
            }

            game.Start(startingPlayer);
            return game;
        }

        public Game StartGameWithPlayPile(ICollection<Player> players, Player startingPlayer, IEnumerable<Card> cardsInPile)
        {
            var game = new Game(players, new RulesProcessesor(_rulesForCardsByValue), _deck, cardsInPile);
            game.Start(startingPlayer);
            return game;
        }

        private Deck _deck;

        private Dictionary<CardValue, RuleForCard> _rulesForCardsByValue;

        private ICanStartGame canStartGame;
    }
}