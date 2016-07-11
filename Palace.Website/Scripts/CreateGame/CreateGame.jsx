 var CardTypes = ["Two",
	"Three",
	"Four",
	"Five",
	"Six",
	"Seven",
	"Eight",
	"Nine",
	"Ten",
	"Jack",
	"Queen",
	"King",
	"Ace"];
    
 var RuleForCard = [
    "LowerThan",
    "Reset",
    "ReverseOrderOfPlay",
    "Burn",
    "SkipPlayer",
    "SeeThrough"];
    
    var Player = React.createClass({
    render: function(){
        return (
            <div>Player name: 
                    Edit: <input type="text" value={this.props.player.name} onChange={this.props.handleChange} />
                    <button onClick={this.props.removePlayer}>Remove</button>
                </div>
        );
    } 
    });

    var CreateGame = React.createClass({
        getInitialState: function(){
            return {
                players: [{name: "Ed"}, {name: "Sophie"}],
                rules: [
                    {rule: "Reset", card: "Two"},
                    {rule: "Burn", card: "Ten"}
                    ]
            };
        },
        convertStateToCommand: function(state){
            var players = [];
            state.players.forEach(function(player){
                players.push(player.name); 
            });
            
            var rules = [];
            state.rules.forEach(function(rule){
               rules.push({
                   CardValue: rule.card,
                   Rule: rule.rule
               }) ;
            });
            return { 
                Players: players,
                Rules: rules
            };
        },
        changePlayerName: function(playerIndex, event){
            this.state.players[playerIndex].name = event.target.value;
            this.setState({
                players: this.state.players
            });
        },  
        sendCommand: function(){
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
        addPlayer: function(){
           this.state.players.push({name: "Player " + (this.state.players.length + 1)});  
           this.setState({
                players: this.state.players
            });
        },
        removePlayer: function(playerIndex){
            //window.alert("Index: " + playerIndex)
            this.state.players.splice(playerIndex, 1);
            this.setState({
                players: this.state.players
            });
        },
        updateCardForRule: function(ruleIndex, event){
            this.state.rules[ruleIndex].card = event.target.value;
            this.setState({
                rules: this.state.rules
            });
        },
        updateRuleForRule: function(ruleIndex, event){
            this.state.rules[ruleIndex].rule = event.target.value;
            this.setState({
                rules: this.state.rules
            });
        },
        getRemainingRules: function(){
            // var rulesToReturn = [];
            // RuleForCard.forEach(function(rule){
            //    if(!this.state.rules.indexOf(rule) > -1){
            //        rulesToReturn.push(rule);
            //    } 
            // });
            //return _.difference(RuleForCard, rulesFromState);   
            
            return RuleForCard;
        },
        addRule: function(){
            this.state.rules.push({rule: "Reset", card: "Two"});
            this.setState({
                rules: this.state.rules
            });
        },
        removeRule: function(ruleIndex){
            this.state.rules.splice(ruleIndex, 1);
            this.setState({
                rules: this.state.rules
            });
        },
        render: function () {
            var players = [];
            var createGame = this;
            this.state.players.forEach(function(player, index){
                players.push(
                    <Player 
                        player={player} 
                        key={index} 
                        handleChange={createGame.changePlayerName.bind(null, index)}
                        removePlayer = {createGame.removePlayer.bind(null, index)}   
                     />
                );
            });
            
            var rulesHtml = [];
            this.state.rules.forEach(function(ruleFromState, index){
                rulesHtml.push(
                    <span key={index}>
                        <select value={ruleFromState.card} onChange={createGame.updateCardForRule.bind(null, index)}>
                            {CardTypes.map(function(cardType, i){
                                var keyForCard = "card-" + i;
                                return <option key={keyForCard} value={cardType}>{cardType}</option>;
                            })}
                        </select>
                        <select value={ruleFromState.rule}  onChange={createGame.updateRuleForRule.bind(null, index)}>
                            {createGame.getRemainingRules()
                                .map(function(ruleType, j){
                                        var keyForCardRule = "ruleCard-" + j;
                                        return <option key={keyForCardRule} value={ruleType}>{ruleType}</option>;
                                    }
                                )
                            }
                        </select>
                        <button onClick={createGame.removeRule.bind(null, index)}>Remove</button>
                        <br/>
                    </span>
                    ); 
            });
            return (
                <div>Hello! Players: 
                    {players}
                    <button onClick={this.addPlayer}>Add player</button>
                    <br/>
                    <h1>Rules</h1>
                    {rulesHtml}
                    <br/>
                    <button onClick={this.addRule}>Add rule</button>
                    <br/>
                    <button onClick={this.sendCommand}>Create game</button>
                </div>
                );
        }
    });

    ReactDOM.render(
        <CreateGame url={PalaceConfig.createGameUrl} />,
        document.getElementById("reactContent")
    );