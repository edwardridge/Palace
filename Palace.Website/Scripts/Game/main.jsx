class Game extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            gameStatusForPlayer: { CardsInHand: [], CardsFaceUp: [], PlayPile: []},
            gameStatusForOpponents: [ { CardsFaceUp: [] } ],
            errors: [],
            gameOver: false,
            rules: []
        }
    }
    
    toggleSelectedVisibleCard(cardPile, index) {
        var deselectCards = function(cards) {
            cards.forEach(function (card) {
                card.selected = false;
            });
        };
        
        var selectedCardsInPile = function(cards){
            var selectedCards = [];
            cards.forEach(function (cardInPile) {
                if (cardInPile.selected) {
                    selectedCards.push(cardInPile);
                }
            });
            
            return selectedCards;
        }
        
        var cardToToggle = this.state.gameStatusForPlayer[cardPile][index];
        var newSelectedValue = !cardToToggle.selected;
        
        var selectedCards = selectedCardsInPile(this.state.gameStatusForPlayer[cardPile]);
        if (selectedCards.length > 0 && selectedCards[0].Value !== cardToToggle.Value && newSelectedValue) {
            deselectCards(selectedCards);
        }

        var otherPile = cardPile === 'CardsFaceUp' ? 'CardsInHand' : 'CardsFaceUp';

        var selectedCardsForOtherPile = selectedCardsInPile(this.state.gameStatusForPlayer[otherPile]);
        if (selectedCardsForOtherPile.length > 0  && newSelectedValue) {
            deselectCards(selectedCardsForOtherPile);
        }
        
        cardToToggle.selected = newSelectedValue;
        this.setState({ gameStatusForPlayer: this.state.gameStatusForPlayer });
    }
    
    playCards(cardPile){
        var game = this;
        var cardsToPlay = [];
        if(this.state.gameStatusForPlayer[cardPile])
        {
            this.state.gameStatusForPlayer[cardPile].forEach(function(card){
            if(card.selected){
                cardsToPlay.push(card);
            }
        });
        }
        
        var methodToSend = '';
        switch(cardPile){
            case 'CardsInHand': methodToSend = game.props.palaceConfig.playInHandCard;
                break;
            case 'CardsFaceUp': methodToSend = game.props.palaceConfig.playFaceUpCard;
                break;
            case 'CardsFaceDown': methodToSend = game.props.palaceConfig.playFaceDownCard;
                break;
            default: throw new Error();
        }
        this.sendPostRequestAndUpdateState(game, methodToSend, JSON.stringify(cardsToPlay));
    }
    
    cannotPlayCards(event){
        var game = this;
        this.sendPostRequestAndUpdateState(game, game.props.palaceConfig.cannotPlayCard);
    }
    
    sendPostRequestAndUpdateState(game, url, postData){
        var xhr = new XMLHttpRequest();
        xhr.open('post', url,  true);
        xhr.setRequestHeader("Content-type", "application/json");
        xhr.onload = function() {
            game.updateStateFromResult(game, xhr.responseText, true);
        };
        xhr.send(postData);
    }
    
    loadCardsFromServer(forceUpdate) {
        var game = this;
        var xhr = new XMLHttpRequest();
        xhr.open('get', game.props.palaceConfig.getUrl, true);
        xhr.onload = function() {
            game.updateStateFromResult(game, xhr.responseText, forceUpdate);
        };
        xhr.send();
   }
   
   loadRulesFromServer(){
       var game = this;
       var xhr = new XMLHttpRequest();
       xhr.open('get', game.props.palaceConfig.getRulesUrl, true);
       xhr.onload = function() {
           var data = JSON.parse(xhr.responseText);
           game.setState({ rules: data.RuleList });
       };
       xhr.send();
    }
   
    updateStateFromResult(game, responseText, forceUpdate){
        var data = JSON.parse(responseText);
        if(forceUpdate || game.state.gameStatusForPlayer.length === 0 || data.GameStatusForPlayer.NumberOfValidMoves > game.state.gameStatusForPlayer.NumberOfValidMoves){
            var newGameStatusForPlayer = game.preserveSelectedCards(data.GameStatusForPlayer, game.state.gameStatusForPlayer);
            game.setState({ gameStatusForPlayer: newGameStatusForPlayer, gameStatusForOpponents: data.GameStatusForPlayer.GameStatusForOpponents, errors: data.Errors, gameOver: data.GameOver });
        }
    }
    
    preserveSelectedCards(newGameStatusForPlayer, oldGameStatusForPlayer) {
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
    
    componentDidMount() {
        this.loadRulesFromServer();
        this.loadCardsFromServer(true);
        window.setInterval(this.loadCardsFromServer.bind(this), this.props.pollInterval);
    }
    
    render(){
       var errors = this.state.errors;
        return (
            <div>
                <GameStatusForPlayer 
                            gameState={this.state.gameStatusForPlayer} 
                            gameOver = {this.state.gameOver}
                            toggleCardSelected={this.toggleSelectedVisibleCard.bind(this)} 
                            playCards={this.playCards.bind(this)} 
                            errors = {errors}
                            cannotPlayCards={this.cannotPlayCards.bind(this)} />
                            
                <GameStatusForOpponents
                            gameState={this.state.gameStatusForOpponents}
                            currentPlayer={this.state.gameStatusForPlayer.CurrentPlayer}
                            />
                            
                <GameRules rules={this.state.rules} />   
            </div> 
        )
    }
};

ReactDOM.render(

  <Game palaceConfig={PalaceConfig} pollInterval={2000} />,
  document.getElementById('reactContent')
);