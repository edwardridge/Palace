"use strict";

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var VisibleCardPile = function (_React$Component) {
    _inherits(VisibleCardPile, _React$Component);

    function VisibleCardPile() {
        _classCallCheck(this, VisibleCardPile);

        return _possibleConstructorReturn(this, Object.getPrototypeOf(VisibleCardPile).apply(this, arguments));
    }

    _createClass(VisibleCardPile, [{
        key: "render",
        value: function render() {
            var emptyFunction = function emptyFunction() {};

            return React.createElement(
                "span",
                null,
                this.props.cards.map(function (card, index) {
                    return React.createElement(
                        "span",
                        { key: index, onClick: this.props.toggleCardSelected ? this.props.toggleCardSelected.bind(null, index) : emptyFunction },
                        React.createElement(Card, { cardVal: card, index: index })
                    );
                }, this)
            );
        }
    }]);

    return VisibleCardPile;
}(React.Component);

;