'use strict';

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var GameStatusForPlayer = function (_React$Component) {
    _inherits(GameStatusForPlayer, _React$Component);

    function GameStatusForPlayer() {
        _classCallCheck(this, GameStatusForPlayer);

        return _possibleConstructorReturn(this, Object.getPrototypeOf(GameStatusForPlayer).apply(this, arguments));
    }

    _createClass(GameStatusForPlayer, [{
        key: 'render',
        value: function render() {
            var gameOver = this.props.gameOver;
            var gameOverMessage = this.props.gameOver ? React.createElement(
                'h2',
                null,
                'GAME OVER! The winner is ',
                this.props.gameState.CurrentPlayer
            ) : null;
            var isPlayersTurn = this.props.gameState.CurrentPlayer === this.props.gameState.Name;

            var noSelectedCards = true;
            this.props.gameState.CardsInHand.forEach(function (card) {
                if (card.selected) {
                    noSelectedCards = false;
                };
            });

            var noFaceUpSelectedCards = true;
            this.props.gameState.CardsFaceUp.forEach(function (card) {
                if (card.selected) {
                    noFaceUpSelectedCards = false;
                };
            });

            var fewerThanThreeCardsInHand = this.props.gameState.CardsInHand.length < 3;

            var otherPlayerText = 'It is ' + this.props.gameState.CurrentPlayer + '\'s go';
            var playersTurn = React.createElement(
                'div',
                null,
                isPlayersTurn ? 'It is your go' : otherPlayerText
            );

            var emptyFunction = function emptyFunction() {};

            var faceUpCardButton = null;
            if (fewerThanThreeCardsInHand) {
                faceUpCardButton = React.createElement(
                    'button',
                    { onClick: this.props.playCards.bind(null, 'CardsFaceUp'), disabled: noFaceUpSelectedCards || !isPlayersTurn || gameOver },
                    'Play cards'
                );
            }

            var isPlayersTurnClass = isPlayersTurn ? 'playersTurn row' : 'opponentsTurn row';

            return React.createElement(
                'div',
                null,
                React.createElement(
                    'div',
                    null,
                    gameOverMessage
                ),
                React.createElement(
                    'div',
                    { className: 'errors' },
                    this.props.errors
                ),
                React.createElement(
                    'h2',
                    null,
                    playersTurn
                ),
                React.createElement(
                    'h2',
                    null,
                    this.props.gameState.Name,
                    ' (you)'
                ),
                React.createElement(
                    'span',
                    { className: isPlayersTurnClass },
                    React.createElement(
                        'span',
                        { className: 'col-xs-12 col-sm-6 col-md-6' },
                        'Cards in hand ',
                        React.createElement('br', null),
                        React.createElement(VisibleCardPile, { cards: this.props.gameState.CardsInHand, toggleCardSelected: this.props.toggleCardSelected.bind(null, 'CardsInHand') }),
                        ' ',
                        React.createElement('br', null),
                        React.createElement(
                            'button',
                            { onClick: this.props.playCards.bind(null, 'CardsInHand'), disabled: noSelectedCards || !isPlayersTurn || gameOver },
                            'Play cards'
                        ),
                        ' ',
                        React.createElement('br', null),
                        ' ',
                        React.createElement('br', null),
                        React.createElement(CannotPlay, { cannotPlayCards: this.props.cannotPlayCards, allowed: !isPlayersTurn || gameOver }),
                        ' ',
                        React.createElement('br', null),
                        ' ',
                        React.createElement('br', null)
                    ),
                    React.createElement(
                        'span',
                        { className: 'col-xs-12 col-sm-6 col-md-6' },
                        React.createElement(
                            'span',
                            { className: 'col-xs-6 col-sm-6 col-md-6' },
                            'Face up cards ',
                            React.createElement('br', null),
                            React.createElement(VisibleCardPile, {
                                cards: this.props.gameState.CardsFaceUp,
                                toggleCardSelected: this.props.toggleCardSelected.bind(null, 'CardsFaceUp') }),
                            faceUpCardButton
                        ),
                        React.createElement(
                            'span',
                            { className: 'col-xs-6 col-sm-6 col-md-6' },
                            'Face down cards ',
                            React.createElement('br', null),
                            React.createElement(FaceDownCards, { cardsCount: this.props.gameState.CardsFaceDownNum, playCards: isPlayersTurn && !gameOver ? this.props.playCards.bind(null, 'CardsFaceDown') : emptyFunction }),
                            ' ',
                            React.createElement('br', null)
                        )
                    )
                ),
                React.createElement(
                    'h2',
                    null,
                    'Play pile'
                ),
                React.createElement(
                    'span',
                    { className: 'row playPile' },
                    React.createElement(
                        'span',
                        { className: 'col-xs-12 col-sm-12 col-md-12' },
                        React.createElement(VisibleCardPile, { cards: this.props.gameState.PlayPile })
                    )
                )
            );
        }
    }]);

    return GameStatusForPlayer;
}(React.Component);

;