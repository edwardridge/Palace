namespace Palace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Dealer
    {
        public Dealer(IEnumerable<Player> players, Deck deck, ICanStartGame canStartGame)
            : this(players, deck, canStartGame, new Dictionary<CardValue, RuleForCard>())
        {
        }

        public Dealer(IEnumerable<Player> players, Deck deck, ICanStartGame canStartGame, Dictionary<CardValue, RuleForCard> rulesForCardsByValue)
        {
            this._players = players.ToList();
            this._deck = deck;
            this._rulesForCardsByValue = rulesForCardsByValue;
            this._canStartGame = canStartGame;
        }

        public void DealIntialCards()
        {
            foreach (var player in _players)
            {
                player.AddCardToFaceDownPile(_deck.DealCards(3));
                player.AddCardsToInHandPile(_deck.DealCards(6));
            }
        }

        public Game StartGame(Player startingPlayer = null)
        {
            if (!this._canStartGame.IsReady(_players))
                throw new InvalidOperationException("The game is not ready to start");

            var game = new Game(_players, new RulesProcessesor(_rulesForCardsByValue), _deck);
            if (startingPlayer == null)
            {
                startingPlayer = _players.First();

                foreach (var player in _players)
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

        private readonly ICollection<Player> _players;

        private Deck _deck;

        private Dictionary<CardValue, RuleForCard> _rulesForCardsByValue;

        private ICanStartGame _canStartGame;
    }
}