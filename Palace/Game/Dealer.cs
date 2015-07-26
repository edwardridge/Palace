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

        public GameInitialisation CreateGameInitialisation()
        {
            return new GameInitialisation(_players, _deck, _canStartGame, _rulesForCardsByValue);
        }

        private readonly ICollection<Player> _players;

        private Deck _deck;

        private Dictionary<CardValue, RuleForCard> _rulesForCardsByValue;

        private ICanStartGame _canStartGame;
    }

    public class GameInitialisation
    {
        private ICanStartGame _canStartGame;
        private Deck _deck;
        private ICollection<Player> _players;
        private Dictionary<CardValue, RuleForCard> _rulesForCardsByValue;

        public GameInitialisation(ICollection<Player> _players, Deck _deck, ICanStartGame _canStartGame, Dictionary<CardValue, RuleForCard> _rulesForCardsByValue)
        {
            this._players = _players;
            this._canStartGame = _canStartGame;
            this._deck = _deck;
            this._rulesForCardsByValue = _rulesForCardsByValue;
        }

        public void DealInitialCards()
        {
            foreach (var player in _players)
            {
                player.AddCardToFaceDownPile(_deck.DealCards(3));
                player.AddCardsToInHandPile(_deck.DealCards(6));
            }
        }

        public Result PutCardFaceUp(Player player, Card cardToPutFaceUp, Card faceUpCardToSwap = null)
        {
            var playerInternal = _players.First(p => p.Name.Equals(player.Name));
            return playerInternal.PutCardFaceUp(cardToPutFaceUp, faceUpCardToSwap);
        }

        public void PlayerReady(Player player)
        {
            var playerInternal = _players.First(p => p.Name.Equals(player.Name));
            playerInternal.Ready();
        }

        public Game StartGame(Player startingPlayer = null)
        {
            if (!this._canStartGame.IsReady(_players))
                throw new InvalidOperationException("The game is not ready to start");
            var id = Guid.NewGuid();

            var gameState = GameState.SetUpInitialState(_players, _deck, id);

            var game = new Game(gameState, new RulesProcessorGenerator(id, _rulesForCardsByValue));
            if (startingPlayer == null)
            {
                startingPlayer = _players.First();

                foreach (var player in _players)
                {
                    if (player.CardsInHand == null || player.CardsInHand.Count == 0)
                        continue;
                    var resetCardValue = _rulesForCardsByValue.FirstOrDefault(rule => rule.Value == RuleForCard.Reset).Key;
                    var lowestCardValueForCurrentPlayer = GetLowestNonResetCardValueForPlayer(player, resetCardValue);
                    var lowestCardValueForStartingPlayer = GetLowestNonResetCardValueForPlayer(startingPlayer, resetCardValue);

                    if (lowestCardValueForCurrentPlayer < lowestCardValueForStartingPlayer)
                        startingPlayer = player;
                }
            }

            game.Start(startingPlayer);
            return game;
        }

        public Game StartGameWithPlayPile(Player startingPlayer, IEnumerable<Card> cardsInPile)
        {
            var id = Guid.NewGuid();

            var gameState = GameState.SetUpInitialState(_players, _deck, id, startingPlayer, cardsInPile);

            var game = new Game(gameState, new RulesProcessorGenerator(id, _rulesForCardsByValue));
            game.Start(startingPlayer);
            return game;
        }

        private static CardValue GetLowestNonResetCardValueForPlayer(Player player, CardValue resetCardValue)
        {
            var lowestNonResetCardValue = player.CardsInHand.Where(card => card.Value != resetCardValue).OrderBy(o => o.Value).First().Value;
            return lowestNonResetCardValue;
        }
    }
}