'use strict';

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _VisibleCardPile = require('./VisibleCardPile.jsx');

var _VisibleCardPile2 = _interopRequireDefault(_VisibleCardPile);

var _FaceDownCards = require('./FaceDownCards.jsx');

var _FaceDownCards2 = _interopRequireDefault(_FaceDownCards);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var GameStatusForOpponent = function (_React$Component) {
    _inherits(GameStatusForOpponent, _React$Component);

    function GameStatusForOpponent() {
        _classCallCheck(this, GameStatusForOpponent);

        return _possibleConstructorReturn(this, Object.getPrototypeOf(GameStatusForOpponent).apply(this, arguments));
    }

    _createClass(GameStatusForOpponent, [{
        key: 'render',
        value: function render() {
            var isPlayersTurn = this.props.name === this.props.currentPlayer;
            var isPlayersTurnClass = isPlayersTurn ? 'playersTurn row' : 'opponentsTurn row';
            return React.createElement(
                'span',
                null,
                React.createElement(
                    'h2',
                    null,
                    this.props.name
                ),
                React.createElement(
                    'span',
                    { className: isPlayersTurnClass },
                    React.createElement(
                        'span',
                        { className: 'col-xs-12 col-sm-6 col-md-6' },
                        'In hand cards ',
                        React.createElement('br', null),
                        React.createElement(_FaceDownCards2.default, { cardsCount: this.props.cardsInHandNum }),
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
                            React.createElement(_VisibleCardPile2.default, { cards: this.props.cardsFaceUp }),
                            ' ',
                            React.createElement('br', null)
                        ),
                        React.createElement(
                            'span',
                            { className: 'col-xs-6 col-sm-6 col-md-6' },
                            'Face down cards ',
                            React.createElement('br', null),
                            React.createElement(_FaceDownCards2.default, { cardsCount: this.props.cardsFaceDownNum }),
                            ' ',
                            React.createElement('br', null)
                        )
                    )
                )
            );
        }
    }]);

    return GameStatusForOpponent;
}(React.Component);

;

exports.default = GameStatusForOpponent;