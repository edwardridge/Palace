"use strict";

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

Object.defineProperty(exports, "__esModule", {
    value: true
});

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var GameRules = function (_React$Component) {
    _inherits(GameRules, _React$Component);

    function GameRules() {
        _classCallCheck(this, GameRules);

        return _possibleConstructorReturn(this, Object.getPrototypeOf(GameRules).apply(this, arguments));
    }

    _createClass(GameRules, [{
        key: "render",
        value: function render() {
            var ruleList = [];
            this.props.rules.forEach(function (rule, index) {
                ruleList.push(React.createElement(
                    "div",
                    { key: index },
                    rule.CardValue,
                    " : ",
                    rule.RuleForCard,
                    " "
                ));
            });

            return React.createElement(
                "div",
                null,
                React.createElement(
                    "h2",
                    null,
                    "RULES"
                ),
                React.createElement(
                    "span",
                    null,
                    ruleList,
                    " "
                )
            );
        }
    }]);

    return GameRules;
}(React.Component);

;

exports.default = GameRules;