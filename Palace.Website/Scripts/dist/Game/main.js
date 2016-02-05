'use strict';

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Game = function (_React$Component) {
    _inherits(Game, _React$Component);

    function Game(props) {
        _classCallCheck(this, Game);

        var _this = _possibleConstructorReturn(this, Object.getPrototypeOf(Game).call(this, props));

        _this.toggleSelectedVisibleCard = function (cardPile, index) {
            var deselectCards = function deselectCards(cards) {
                cards.forEach(function (card) {
                    card.selected = false;
                });
            };

            var selectedCardsInPile = function selectedCardsInPile(cards) {
                var selectedCards = [];
                cards.forEach(function (cardInPile) {
                    if (cardInPile.selected) {
                        selectedCards.push(cardInPile);
                    }
                });

                return selectedCards;
            };

            var cardToToggle = _this.state.gameStatusForPlayer[cardPile][index];
            var newSelectedValue = !cardToToggle.selected;

            var selectedCards = selectedCardsInPile(_this.state.gameStatusForPlayer[cardPile]);
            if (selectedCards.length > 0 && selectedCards[0].Value !== cardToToggle.Value && newSelectedValue) {
                deselectCards(selectedCards);
            }

            var otherPile = cardPile === 'CardsFaceUp' ? 'CardsInHand' : 'CardsFaceUp';

            var selectedCardsForOtherPile = selectedCardsInPile(_this.state.gameStatusForPlayer[otherPile]);
            if (selectedCardsForOtherPile.length > 0 && newSelectedValue) {
                deselectCards(selectedCardsForOtherPile);
            }

            cardToToggle.selected = newSelectedValue;
            _this.setState({ gameStatusForPlayer: _this.state.gameStatusForPlayer });
        };

        _this.playCards = function (cardPile) {
            var game = _this;
            var cardsToPlay = [];
            if (_this.state.gameStatusForPlayer[cardPile]) {
                _this.state.gameStatusForPlayer[cardPile].forEach(function (card) {
                    if (card.selected) {
                        cardsToPlay.push(card);
                    }
                });
            }

            var methodToSend = '';
            switch (cardPile) {
                case 'CardsInHand':
                    methodToSend = game.props.palaceConfig.playInHandCard;
                    break;
                case 'CardsFaceUp':
                    methodToSend = game.props.palaceConfig.playFaceUpCard;
                    break;
                case 'CardsFaceDown':
                    methodToSend = game.props.palaceConfig.playFaceDownCard;
                    break;
                default:
                    throw new Error();
            }
            _this.sendPostRequestAndUpdateState(game, methodToSend, JSON.stringify(cardsToPlay));
        };

        _this.cannotPlayCards = function (event) {
            var game = _this;
            _this.sendPostRequestAndUpdateState(game, game.props.palaceConfig.cannotPlayCard);
        };

        _this.loadCardsFromServer = function (forceUpdate) {
            var game = _this;
            var xhr = new XMLHttpRequest();
            xhr.open('get', game.props.palaceConfig.getUrl, true);
            xhr.onload = function () {
                game.updateStateFromResult(game, xhr.responseText, forceUpdate);
            };
            xhr.send();
        };

        _this.loadRulesFromServer = function () {
            var game = _this;
            var xhr = new XMLHttpRequest();
            xhr.open('get', game.props.palaceConfig.getRulesUrl, true);
            xhr.onload = function () {
                var data = JSON.parse(xhr.responseText);
                game.setState({ rules: data.RuleList });
            };
            xhr.send();
        };

        _this.state = {
            gameStatusForPlayer: { CardsInHand: [], CardsFaceUp: [], PlayPile: [] },
            gameStatusForOpponents: [{ CardsFaceUp: [] }],
            errors: [],
            gameOver: false,
            rules: []
        };
        return _this;
    }

    _createClass(Game, [{
        key: 'sendPostRequestAndUpdateState',
        value: function sendPostRequestAndUpdateState(game, url, postData) {
            var xhr = new XMLHttpRequest();
            xhr.open('post', url, true);
            xhr.setRequestHeader("Content-type", "application/json");
            xhr.onload = function () {
                game.updateStateFromResult(game, xhr.responseText, true);
            };
            xhr.send(postData);
        }
    }, {
        key: 'updateStateFromResult',
        value: function updateStateFromResult(game, responseText, forceUpdate) {
            var data = JSON.parse(responseText);
            if (forceUpdate || game.state.gameStatusForPlayer.length === 0 || data.GameStatusForPlayer.NumberOfValidMoves > game.state.gameStatusForPlayer.NumberOfValidMoves) {
                var newGameStatusForPlayer = game.preserveSelectedCards(data.GameStatusForPlayer, game.state.gameStatusForPlayer);
                game.setState({ gameStatusForPlayer: newGameStatusForPlayer, gameStatusForOpponents: data.GameStatusForPlayer.GameStatusForOpponents, errors: data.Errors, gameOver: data.GameOver });
            }
        }
    }, {
        key: 'preserveSelectedCards',
        value: function preserveSelectedCards(newGameStatusForPlayer, oldGameStatusForPlayer) {
            //If we have just played, don't preserve selected cards
            if (oldGameStatusForPlayer.Name === oldGameStatusForPlayer.CurrentPlayer) {
                return newGameStatusForPlayer;
            }
            oldGameStatusForPlayer.CardsInHand.forEach(function (card, index) {
                newGameStatusForPlayer.CardsInHand[index].selected = card.selected;
            });

            oldGameStatusForPlayer.CardsFaceUp.forEach(function (card, index) {
                newGameStatusForPlayer.CardsFaceUp[index].selected = card.selected;
            });

            return newGameStatusForPlayer;
        }
    }, {
        key: 'componentDidMount',
        value: function componentDidMount() {
            this.loadRulesFromServer();
            this.loadCardsFromServer(true);
            window.setInterval(this.loadCardsFromServer, this.props.pollInterval);
        }
    }, {
        key: 'render',
        value: function render() {
            var errors = this.state.errors;
            return React.createElement(
                'div',
                null,
                React.createElement(GameStatusForPlayer, {
                    gameState: this.state.gameStatusForPlayer,
                    gameOver: this.state.gameOver,
                    toggleCardSelected: this.toggleSelectedVisibleCard,
                    playCards: this.playCards,
                    errors: errors,
                    cannotPlayCards: this.cannotPlayCards }),
                React.createElement(GameStatusForOpponents, {
                    gameState: this.state.gameStatusForOpponents,
                    currentPlayer: this.state.gameStatusForPlayer.CurrentPlayer
                }),
                React.createElement(GameRules, { rules: this.state.rules })
            );
        }
    }]);

    return Game;
}(React.Component);

;

//export default Game;

ReactDOM.render(React.createElement(Game, { palaceConfig: PalaceConfig, pollInterval: 2000 }), document.getElementById('reactContent'));