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
/***/ function(module, exports) {

	"use strict";

	var CardTypes = ["Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace"];

	var RuleForCard = ["LowerThan", "Reset", "ReverseOrderOfPlay", "Burn", "SkipPlayer", "SeeThrough"];

	var Player = React.createClass({
	    displayName: "Player",

	    render: function render() {
	        return React.createElement(
	            "div",
	            null,
	            "Player name: Edit: ",
	            React.createElement("input", { type: "text", value: this.props.player.name, onChange: this.props.handleChange }),
	            React.createElement(
	                "button",
	                { onClick: this.props.removePlayer },
	                "Remove"
	            )
	        );
	    }
	});

	var CreateGame = React.createClass({
	    displayName: "CreateGame",

	    getInitialState: function getInitialState() {
	        return {
	            players: [{ name: "Ed" }, { name: "Sophie" }],
	            rules: [{ rule: "Reset", card: "Two" }, { rule: "Burn", card: "Ten" }]
	        };
	    },
	    convertStateToCommand: function convertStateToCommand(state) {
	        var players = [];
	        state.players.forEach(function (player) {
	            players.push(player.name);
	        });

	        var rules = [];
	        state.rules.forEach(function (rule) {
	            rules.push({
	                CardValue: rule.card,
	                Rule: rule.rule
	            });
	        });
	        return {
	            Players: players,
	            Rules: rules
	        };
	    },
	    changePlayerName: function changePlayerName(playerIndex, event) {
	        this.state.players[playerIndex].name = event.target.value;
	        this.setState({
	            players: this.state.players
	        });
	    },
	    sendCommand: function sendCommand() {
	        var createGame = this;
	        var url = this.props.url;

	        var command = createGame.convertStateToCommand(createGame.state);
	        var data = JSON.stringify(command);
	        var xhr = new XMLHttpRequest();
	        xhr.open("post", url, true);
	        xhr.setRequestHeader("Content-type", "application/json");
	        xhr.onload = function () {
	            window.alert("Game created!");
	        };
	        xhr.send(data);
	    },
	    addPlayer: function addPlayer() {
	        this.state.players.push({ name: "Player " + (this.state.players.length + 1) });
	        this.setState({
	            players: this.state.players
	        });
	    },
	    removePlayer: function removePlayer(playerIndex) {
	        //window.alert("Index: " + playerIndex)
	        this.state.players.splice(playerIndex, 1);
	        this.setState({
	            players: this.state.players
	        });
	    },
	    updateCardForRule: function updateCardForRule(ruleIndex, event) {
	        this.state.rules[ruleIndex].card = event.target.value;
	        this.setState({
	            rules: this.state.rules
	        });
	    },
	    updateRuleForRule: function updateRuleForRule(ruleIndex, event) {
	        this.state.rules[ruleIndex].rule = event.target.value;
	        this.setState({
	            rules: this.state.rules
	        });
	    },
	    getRemainingRules: function getRemainingRules() {
	        // var rulesToReturn = [];
	        // RuleForCard.forEach(function(rule){
	        //    if(!this.state.rules.indexOf(rule) > -1){
	        //        rulesToReturn.push(rule);
	        //    }
	        // });
	        //return _.difference(RuleForCard, rulesFromState);  

	        return RuleForCard;
	    },
	    addRule: function addRule() {
	        this.state.rules.push({ rule: "Reset", card: "Two" });
	        this.setState({
	            rules: this.state.rules
	        });
	    },
	    removeRule: function removeRule(ruleIndex) {
	        this.state.rules.splice(ruleIndex, 1);
	        this.setState({
	            rules: this.state.rules
	        });
	    },
	    render: function render() {
	        var players = [];
	        var createGame = this;
	        this.state.players.forEach(function (player, index) {
	            players.push(React.createElement(Player, {
	                player: player,
	                key: index,
	                handleChange: createGame.changePlayerName.bind(null, index),
	                removePlayer: createGame.removePlayer.bind(null, index)
	            }));
	        });

	        var rulesHtml = [];
	        this.state.rules.forEach(function (ruleFromState, index) {
	            rulesHtml.push(React.createElement(
	                "span",
	                { key: index },
	                React.createElement(
	                    "select",
	                    { value: ruleFromState.card, onChange: createGame.updateCardForRule.bind(null, index) },
	                    CardTypes.map(function (cardType, i) {
	                        var keyForCard = "card-" + i;
	                        return React.createElement(
	                            "option",
	                            { key: keyForCard, value: cardType },
	                            cardType
	                        );
	                    })
	                ),
	                React.createElement(
	                    "select",
	                    { value: ruleFromState.rule, onChange: createGame.updateRuleForRule.bind(null, index) },
	                    createGame.getRemainingRules().map(function (ruleType, j) {
	                        var keyForCardRule = "ruleCard-" + j;
	                        return React.createElement(
	                            "option",
	                            { key: keyForCardRule, value: ruleType },
	                            ruleType
	                        );
	                    })
	                ),
	                React.createElement(
	                    "button",
	                    { onClick: createGame.removeRule.bind(null, index) },
	                    "Remove"
	                ),
	                React.createElement("br", null)
	            ));
	        });
	        return React.createElement(
	            "div",
	            null,
	            "Hello! Players:",
	            players,
	            React.createElement(
	                "button",
	                { onClick: this.addPlayer },
	                "Add player"
	            ),
	            React.createElement("br", null),
	            React.createElement(
	                "h1",
	                null,
	                "Rules"
	            ),
	            rulesHtml,
	            React.createElement("br", null),
	            React.createElement(
	                "button",
	                { onClick: this.addRule },
	                "Add rule"
	            ),
	            React.createElement("br", null),
	            React.createElement(
	                "button",
	                { onClick: this.sendCommand },
	                "Create game"
	            )
	        );
	    }
	});

	ReactDOM.render(React.createElement(CreateGame, { url: PalaceConfig.createGameUrl }), document.getElementById("reactContent"));

/***/ }
/******/ ]);