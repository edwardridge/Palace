/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};

/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {

/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId])
/******/ 			return installedModules[moduleId].exports;

/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			exports: {},
/******/ 			id: moduleId,
/******/ 			loaded: false
/******/ 		};

/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);

/******/ 		// Flag the module as loaded
/******/ 		module.loaded = true;

/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}


/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;

/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;

/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";

/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(0);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";

	var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

	Object.defineProperty(exports, "__esModule", {
	    value: true
	});

	var _GameRules = __webpack_require__(1);

	var _GameRules2 = _interopRequireDefault(_GameRules);

	var _GameStatusForPlayer = __webpack_require__(2);

	var _GameStatusForPlayer2 = _interopRequireDefault(_GameStatusForPlayer);

	var _GameStatusForOpponents = __webpack_require__(7);

	var _GameStatusForOpponents2 = _interopRequireDefault(_GameStatusForOpponents);

	function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

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
	                return cards.filter(function (cardInPile) {
	                    return cardInPile.selected;
	                });
	            };

	            var cardToToggle = _this.state.gameStatusForPlayer[cardPile][index];
	            var newSelectedValue = !cardToToggle.selected;

	            var selectedCards = selectedCardsInPile(_this.state.gameStatusForPlayer[cardPile]);
	            if (selectedCards.length > 0 && selectedCards[0].Value !== cardToToggle.Value && newSelectedValue) {
	                deselectCards(selectedCards);
	            }

	            var otherPile = cardPile === "CardsFaceUp" ? "CardsInHand" : "CardsFaceUp";

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
	            if (game.state.gameStatusForPlayer[cardPile]) {
	                cardsToPlay = game.state.gameStatusForPlayer[cardPile].filter(function (card) {
	                    return card.selected;
	                });
	            }

	            var methodToSend = "";
	            switch (cardPile) {
	                case "CardsInHand":
	                    methodToSend = game.props.palaceConfig.playInHandCard;
	                    break;
	                case "CardsFaceUp":
	                    methodToSend = game.props.palaceConfig.playFaceUpCard;
	                    break;
	                case "CardsFaceDown":
	                    methodToSend = game.props.palaceConfig.playFaceDownCard;
	                    break;
	                default:
	                    throw new Error();
	            }
	            game.sendPostRequestAndUpdateState(game, methodToSend, cardsToPlay);
	        };

	        _this.cannotPlayCards = function () {
	            var game = _this;
	            _this.sendPostRequestAndUpdateState(game, game.props.palaceConfig.cannotPlayCard);
	        };

	        _this.loadCardsFromServer = function (forceUpdate) {
	            var game = _this;
	            $.get(game.props.palaceConfig.getUrl, function (result) {
	                game.updateStateFromResult(game, result, forceUpdate);
	            });
	        };

	        _this.loadRulesFromServer = function () {
	            var game = _this;
	            $.get(game.props.palaceConfig.getRulesUrl, function (result) {
	                game.setState({ rules: result.RuleList });
	            });
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
	        key: "sendPostRequestAndUpdateState",
	        value: function sendPostRequestAndUpdateState(game, url, postData) {
	            $.ajax({
	                type: "POST",
	                url: url,
	                data: JSON.stringify(postData),
	                success: function success(result) {
	                    game.updateStateFromResult(game, result, true);
	                },
	                contentType: "application/json",
	                dataType: "json"
	            });
	        }
	    }, {
	        key: "updateStateFromResult",
	        value: function updateStateFromResult(game, data, forceUpdate) {
	            if (forceUpdate || game.state.gameStatusForPlayer.length === 0 || data.GameStatusForPlayer.NumberOfValidMoves > game.state.gameStatusForPlayer.NumberOfValidMoves) {
	                var newGameStatusForPlayer = game.preserveSelectedCards(data.GameStatusForPlayer, game.state.gameStatusForPlayer);
	                game.setState({ gameStatusForPlayer: newGameStatusForPlayer, gameStatusForOpponents: data.GameStatusForPlayer.GameStatusForOpponents, errors: data.Errors, gameOver: data.GameOver });
	            }
	        }
	    }, {
	        key: "preserveSelectedCards",
	        value: function preserveSelectedCards(newGameStatusForPlayer, oldGameStatusForPlayer) {
	            //If we have just played, don"t preserve selected cards
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
	        key: "componentDidMount",
	        value: function componentDidMount() {
	            this.loadRulesFromServer();
	            this.loadCardsFromServer(true);
	            window.setInterval(this.loadCardsFromServer, this.props.pollInterval);
	        }
	    }, {
	        key: "render",
	        value: function render() {
	            return React.createElement(
	                "div",
	                null,
	                React.createElement(_GameStatusForPlayer2.default, {
	                    gameState: this.state.gameStatusForPlayer,
	                    gameOver: this.state.gameOver,
	                    toggleCardSelected: this.toggleSelectedVisibleCard,
	                    playCards: this.playCards,
	                    errors: this.state.errors,
	                    cannotPlayCards: this.cannotPlayCards }),
	                React.createElement(_GameStatusForOpponents2.default, {
	                    gameState: this.state.gameStatusForOpponents,
	                    currentPlayer: this.state.gameStatusForPlayer.CurrentPlayer
	                }),
	                React.createElement(_GameRules2.default, { rules: this.state.rules })
	            );
	        }
	    }]);

	    return Game;
	}(React.Component);

	exports.default = Game;

	ReactDOM.render(React.createElement(Game, { palaceConfig: PalaceConfig, pollInterval: 2000 }), document.getElementById("reactContent"));

/***/ },
/* 1 */
/***/ function(module, exports) {

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

	exports.default = GameRules;

/***/ },
/* 2 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";

	var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

	Object.defineProperty(exports, "__esModule", {
	    value: true
	});

	var _VisibleCardPile = __webpack_require__(3);

	var _VisibleCardPile2 = _interopRequireDefault(_VisibleCardPile);

	var _FaceDownCards = __webpack_require__(5);

	var _FaceDownCards2 = _interopRequireDefault(_FaceDownCards);

	var _CannotPlay = __webpack_require__(6);

	var _CannotPlay2 = _interopRequireDefault(_CannotPlay);

	function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

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
	        key: "render",
	        value: function render() {
	            var gameOver = this.props.gameOver;
	            var gameOverMessage = this.props.gameOver ? React.createElement(
	                "h2",
	                null,
	                "GAME OVER! The winner is ",
	                this.props.gameState.CurrentPlayer
	            ) : null;
	            var isPlayersTurn = this.props.gameState.CurrentPlayer === this.props.gameState.Name;

	            var cardIsNotSelected = function cardIsNotSelected(card) {
	                return !card.selected;
	            };

	            var noSelectedCards = this.props.gameState.CardsInHand.every(cardIsNotSelected);

	            var noFaceUpSelectedCards = this.props.gameState.CardsFaceUp.every(cardIsNotSelected);

	            var fewerThanThreeCardsInHand = this.props.gameState.CardsInHand.length < 3;

	            var otherPlayerText = "It is " + this.props.gameState.CurrentPlayer + "\"s go";
	            var playersTurn = React.createElement(
	                "div",
	                null,
	                isPlayersTurn ? "It is your go" : otherPlayerText
	            );

	            var emptyFunction = function emptyFunction() {};

	            var faceUpCardButton = null;
	            if (fewerThanThreeCardsInHand) {
	                faceUpCardButton = React.createElement(
	                    "button",
	                    { onClick: this.props.playCards.bind(null, "CardsFaceUp"), disabled: noFaceUpSelectedCards || !isPlayersTurn || gameOver },
	                    "Play cards"
	                );
	            }

	            var isPlayersTurnClass = isPlayersTurn ? "playersTurn row" : "opponentsTurn row";

	            return React.createElement(
	                "div",
	                null,
	                React.createElement(
	                    "div",
	                    null,
	                    gameOverMessage
	                ),
	                React.createElement(
	                    "div",
	                    { className: "errors" },
	                    this.props.errors
	                ),
	                React.createElement(
	                    "h2",
	                    null,
	                    playersTurn
	                ),
	                React.createElement(
	                    "h2",
	                    null,
	                    this.props.gameState.Name,
	                    " (you)"
	                ),
	                React.createElement(
	                    "span",
	                    { className: isPlayersTurnClass },
	                    React.createElement(
	                        "span",
	                        { className: "col-xs-12 col-sm-6 col-md-6" },
	                        "Cards in hand ",
	                        React.createElement("br", null),
	                        React.createElement(_VisibleCardPile2.default, { cards: this.props.gameState.CardsInHand, toggleCardSelected: this.props.toggleCardSelected.bind(null, "CardsInHand") }),
	                        " ",
	                        React.createElement("br", null),
	                        React.createElement(
	                            "button",
	                            { onClick: this.props.playCards.bind(null, "CardsInHand"), disabled: noSelectedCards || !isPlayersTurn || gameOver },
	                            "Play cards"
	                        ),
	                        " ",
	                        React.createElement("br", null),
	                        React.createElement(_CannotPlay2.default, { cannotPlayCards: this.props.cannotPlayCards, allowed: !isPlayersTurn || gameOver }),
	                        " ",
	                        React.createElement("br", null)
	                    ),
	                    React.createElement(
	                        "span",
	                        { className: "col-xs-12 col-sm-6 col-md-6" },
	                        React.createElement(
	                            "span",
	                            { className: "col-xs-6 col-sm-6 col-md-6" },
	                            "Face up cards ",
	                            React.createElement("br", null),
	                            React.createElement(_VisibleCardPile2.default, {
	                                cards: this.props.gameState.CardsFaceUp,
	                                toggleCardSelected: this.props.toggleCardSelected.bind(null, "CardsFaceUp") }),
	                            faceUpCardButton
	                        ),
	                        React.createElement(
	                            "span",
	                            { className: "col-xs-6 col-sm-6 col-md-6" },
	                            "Face down cards ",
	                            React.createElement("br", null),
	                            React.createElement(_FaceDownCards2.default, { cardsCount: this.props.gameState.CardsFaceDownNum, playCards: isPlayersTurn && !gameOver ? this.props.playCards.bind(null, "CardsFaceDown") : emptyFunction }),
	                            " ",
	                            React.createElement("br", null)
	                        )
	                    )
	                ),
	                React.createElement(
	                    "h2",
	                    null,
	                    "Play pile"
	                ),
	                React.createElement(
	                    "span",
	                    { className: "row playPile" },
	                    React.createElement(
	                        "span",
	                        { className: "col-xs-12 col-sm-12 col-md-12" },
	                        React.createElement(_VisibleCardPile2.default, { cards: this.props.gameState.PlayPile })
	                    )
	                )
	            );
	        }
	    }]);

	    return GameStatusForPlayer;
	}(React.Component);

	exports.default = GameStatusForPlayer;

/***/ },
/* 3 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";

	var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

	Object.defineProperty(exports, "__esModule", {
	    value: true
	});

	var _Card = __webpack_require__(4);

	var _Card2 = _interopRequireDefault(_Card);

	function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

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
	                        React.createElement(_Card2.default, { cardVal: card, index: index })
	                    );
	                }, this)
	            );
	        }
	    }]);

	    return VisibleCardPile;
	}(React.Component);

	exports.default = VisibleCardPile;

/***/ },
/* 4 */
/***/ function(module, exports) {

	"use strict";

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
	        key: "render",
	        value: function render() {
	            var textToNumber = function textToNumber(text) {
	                switch (text) {
	                    case "one":
	                        return 1;
	                    case "two":
	                        return 2;
	                    case "three":
	                        return 3;
	                    case "four":
	                        return 4;
	                    case "five":
	                        return 5;
	                    case "six":
	                        return 6;
	                    case "seven":
	                        return 7;
	                    case "eight":
	                        return 8;
	                    case "nine":
	                        return 9;
	                    case "ten":
	                        return 10;

	                    default:
	                        return text;
	                }
	            };
	            var imageName = "/Content/cards/" + textToNumber(this.props.cardVal.Value.toLowerCase()) + "_of_" + this.props.cardVal.Suit.toLowerCase() + "s.png";

	            var imageClass = "cardImage ";
	            if (this.props.cardVal.selected) {
	                imageClass = imageClass + "selected";
	            } else {
	                imageClass = imageClass + "not-selected";
	            }

	            return React.createElement(
	                "span",
	                { className: "card" },
	                React.createElement("img", { src: imageName, className: imageClass })
	            );
	        }
	    }]);

	    return Card;
	}(React.Component);

	exports.default = Card;

/***/ },
/* 5 */
/***/ function(module, exports) {

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

	exports.default = FaceDownCards;

/***/ },
/* 6 */
/***/ function(module, exports) {

	"use strict";

	var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

	Object.defineProperty(exports, "__esModule", {
	    value: true
	});

	function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

	function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

	function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

	var CannotPlay = function (_React$Component) {
	    _inherits(CannotPlay, _React$Component);

	    function CannotPlay() {
	        _classCallCheck(this, CannotPlay);

	        return _possibleConstructorReturn(this, Object.getPrototypeOf(CannotPlay).apply(this, arguments));
	    }

	    _createClass(CannotPlay, [{
	        key: "render",
	        value: function render() {
	            return React.createElement(
	                "button",
	                { className: "CannotPlay", onClick: this.props.cannotPlayCards, disabled: this.props.allowed },
	                "I can not play a card!"
	            );
	        }
	    }]);

	    return CannotPlay;
	}(React.Component);

	exports.default = CannotPlay;

/***/ },
/* 7 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";

	var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

	Object.defineProperty(exports, "__esModule", {
	    value: true
	});

	var _GameStatusForOpponent = __webpack_require__(8);

	var _GameStatusForOpponent2 = _interopRequireDefault(_GameStatusForOpponent);

	function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

	function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

	function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

	function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

	var GameStatusForOpponents = function (_React$Component) {
	    _inherits(GameStatusForOpponents, _React$Component);

	    function GameStatusForOpponents() {
	        _classCallCheck(this, GameStatusForOpponents);

	        return _possibleConstructorReturn(this, Object.getPrototypeOf(GameStatusForOpponents).apply(this, arguments));
	    }

	    _createClass(GameStatusForOpponents, [{
	        key: "render",
	        value: function render() {
	            var opponents = [];
	            var gameStatusForOpponents = this;
	            this.props.gameState.forEach(function (opponent, index) {
	                opponents.push(React.createElement(
	                    "div",
	                    { key: index },
	                    React.createElement(_GameStatusForOpponent2.default, {
	                        cardsFaceUp: opponent.CardsFaceUp,
	                        cardsFaceDownNum: opponent.CardsFaceDownNum,
	                        cardsInHandNum: opponent.CardsInHandNum,
	                        name: opponent.Name,
	                        currentPlayer: gameStatusForOpponents.props.currentPlayer
	                    })
	                ));
	            });
	            return React.createElement(
	                "div",
	                null,
	                " ",
	                opponents,
	                " "
	            );
	        }
	    }]);

	    return GameStatusForOpponents;
	}(React.Component);

	exports.default = GameStatusForOpponents;

/***/ },
/* 8 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";

	var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

	Object.defineProperty(exports, "__esModule", {
	    value: true
	});

	var _VisibleCardPile = __webpack_require__(3);

	var _VisibleCardPile2 = _interopRequireDefault(_VisibleCardPile);

	var _FaceDownCards = __webpack_require__(5);

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
	        key: "render",
	        value: function render() {
	            var isPlayersTurn = this.props.name === this.props.currentPlayer;
	            var isPlayersTurnClass = isPlayersTurn ? "playersTurn row" : "opponentsTurn row";
	            return React.createElement(
	                "span",
	                null,
	                React.createElement(
	                    "h2",
	                    null,
	                    this.props.name
	                ),
	                React.createElement(
	                    "span",
	                    { className: isPlayersTurnClass },
	                    React.createElement(
	                        "span",
	                        { className: "col-xs-12 col-sm-6 col-md-6" },
	                        "In hand cards ",
	                        React.createElement("br", null),
	                        React.createElement(_FaceDownCards2.default, { cardsCount: this.props.cardsInHandNum }),
	                        " ",
	                        React.createElement("br", null)
	                    ),
	                    React.createElement(
	                        "span",
	                        { className: "col-xs-12 col-sm-6 col-md-6" },
	                        React.createElement(
	                            "span",
	                            { className: "col-xs-6 col-sm-6 col-md-6" },
	                            "Face up cards ",
	                            React.createElement("br", null),
	                            React.createElement(_VisibleCardPile2.default, { cards: this.props.cardsFaceUp }),
	                            " ",
	                            React.createElement("br", null)
	                        ),
	                        React.createElement(
	                            "span",
	                            { className: "col-xs-6 col-sm-6 col-md-6" },
	                            "Face down cards ",
	                            React.createElement("br", null),
	                            React.createElement(_FaceDownCards2.default, { cardsCount: this.props.cardsFaceDownNum }),
	                            " ",
	                            React.createElement("br", null)
	                        )
	                    )
	                )
	            );
	        }
	    }]);

	    return GameStatusForOpponent;
	}(React.Component);

	exports.default = GameStatusForOpponent;

/***/ }
/******/ ]);