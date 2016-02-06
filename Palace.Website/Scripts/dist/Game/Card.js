'use strict';

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

Object.defineProperty(exports, "__esModule", {
    value: true
});

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Card = function (_React$Component) {
    _inherits(Card, _React$Component);

    function Card() {
        _classCallCheck(this, Card);

        return _possibleConstructorReturn(this, Object.getPrototypeOf(Card).apply(this, arguments));
    }

    _createClass(Card, [{
        key: 'render',
        value: function render() {
            var textToNumber = function textToNumber(text) {
                switch (text) {
                    case 'one':
                        return 1;
                    case 'two':
                        return 2;
                    case 'three':
                        return 3;
                    case 'four':
                        return 4;
                    case 'five':
                        return 5;
                    case 'six':
                        return 6;
                    case 'seven':
                        return 7;
                    case 'eight':
                        return 8;
                    case 'nine':
                        return 9;
                    case 'ten':
                        return 10;

                    default:
                        return text;
                }
            };
            var imageName = '/Content/cards/' + textToNumber(this.props.cardVal.Value.toLowerCase()) + '_of_' + this.props.cardVal.Suit.toLowerCase() + 's.png';

            var imageClass = 'cardImage ';
            if (this.props.cardVal.selected) {
                imageClass = imageClass + 'selected';
            } else {
                imageClass = imageClass + 'not-selected';
            }

            return React.createElement(
                'span',
                { className: 'card' },
                React.createElement('img', { src: imageName, className: imageClass })
            );
        }
    }]);

    return Card;
}(React.Component);

exports.default = Card;