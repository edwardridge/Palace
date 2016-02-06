'use strict';

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

Object.defineProperty(exports, "__esModule", {
    value: true
});

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var FaceDownCards = function (_React$Component) {
    _inherits(FaceDownCards, _React$Component);

    function FaceDownCards() {
        _classCallCheck(this, FaceDownCards);

        return _possibleConstructorReturn(this, Object.getPrototypeOf(FaceDownCards).apply(this, arguments));
    }

    _createClass(FaceDownCards, [{
        key: 'render',
        value: function render() {
            var faceDown = [];
            var emptyFunction = function emptyFunction() {};
            for (var i = 0; i < this.props.cardsCount; i++) {
                faceDown.push(React.createElement(
                    'span',
                    { key: i,
                        onClick: this.props.playCards ? this.props.playCards : emptyFunction },
                    React.createElement('img', { src: '/Content/cards/card_back.png', className: 'cardImage' })
                ));
            }
            return React.createElement(
                'span',
                { className: 'FaceDown' },
                faceDown
            );
        }
    }]);

    return FaceDownCards;
}(React.Component);

;

exports.default = FaceDownCards;