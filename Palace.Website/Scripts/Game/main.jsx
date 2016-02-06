
import GameRules from "./GameRules.jsx";
import GameStatusForPlayer from "./GameStatusForPlayer.jsx";
import GameStatusForOpponents from "./GameStatusForOpponents.jsx";


class Game extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            gameStatusForPlayer: { CardsInHand: [], CardsFaceUp: [], PlayPile: []},
            gameStatusForOpponents: [ { CardsFaceUp: [] } ],
            errors: [],
            gameOver: false,
            rules: []
        };
    }
    
    toggleSelectedVisibleCard = (cardPile, index) => {
        let deselectCards = function(cards) {
            cards.forEach(function (card) {
                card.selected = false;
            });
        };
        
        let selectedCardsInPile = function(cards){
            return cards.filter(function (cardInPile) {
                return cardInPile.selected;
            });
        };
        
        let cardToToggle = this.state.gameStatusForPlayer[cardPile][index];
        let newSelectedValue = !cardToToggle.selected;
        
        let selectedCards = selectedCardsInPile(this.state.gameStatusForPlayer[cardPile]);
        if (selectedCards.length > 0 && selectedCards[0].Value !== cardToToggle.Value && newSelectedValue) {
            deselectCards(selectedCards);
        }

        let otherPile = cardPile === "CardsFaceUp" ? "CardsInHand" : "CardsFaceUp";

        let selectedCardsForOtherPile = selectedCardsInPile(this.state.gameStatusForPlayer[otherPile]);
        if (selectedCardsForOtherPile.length > 0  && newSelectedValue) {
            deselectCards(selectedCardsForOtherPile);
        }
        
        cardToToggle.selected = newSelectedValue;
        this.setState({ gameStatusForPlayer: this.state.gameStatusForPlayer });
    };
    
    playCards = (cardPile) => {
        let game = this;
        let cardsToPlay = [];
        if(game.state.gameStatusForPlayer[cardPile])
        {
            cardsToPlay = game.state.gameStatusForPlayer[cardPile].filter(function(card){
                return card.selected;
            });
        }
        
        let methodToSend = "";
        switch(cardPile){
            case "CardsInHand": methodToSend = game.props.palaceConfig.playInHandCard;
                break;
            case "CardsFaceUp": methodToSend = game.props.palaceConfig.playFaceUpCard;
                break;
            case "CardsFaceDown": methodToSend = game.props.palaceConfig.playFaceDownCard;
                break;
            default: throw new Error();
        }
        game.sendPostRequestAndUpdateState(game, methodToSend, cardsToPlay);
    };
    
    cannotPlayCards = () => {
        let game = this;
        this.sendPostRequestAndUpdateState(game, game.props.palaceConfig.cannotPlayCard);
    };
    
    sendPostRequestAndUpdateState(game, url, postData){
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(postData),
            success: function(result) { game.updateStateFromResult(game, result, true); },
            contentType: "application/json",
            dataType: "json"
        });
    }
    
    loadCardsFromServer = (forceUpdate) => {
        let game = this;
        $.get(game.props.palaceConfig.getUrl, function(result){
            game.updateStateFromResult(game, result, forceUpdate);
        });
   };
   
   loadRulesFromServer = () => {
       let game = this;
        $.get(game.props.palaceConfig.getRulesUrl, function(result){
            game.setState({ rules: result.RuleList });
        });
    };
   
    updateStateFromResult(game, data, forceUpdate){
        if(forceUpdate || game.state.gameStatusForPlayer.length === 0 || data.GameStatusForPlayer.NumberOfValidMoves > game.state.gameStatusForPlayer.NumberOfValidMoves){
            let newGameStatusForPlayer = game.preserveSelectedCards(data.GameStatusForPlayer, game.state.gameStatusForPlayer);
            game.setState({ gameStatusForPlayer: newGameStatusForPlayer, gameStatusForOpponents: data.GameStatusForPlayer.GameStatusForOpponents, errors: data.Errors, gameOver: data.GameOver });
        }
    }
    
    preserveSelectedCards(newGameStatusForPlayer, oldGameStatusForPlayer) {
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
    
    componentDidMount() {
        this.loadRulesFromServer();
        this.loadCardsFromServer(true);
        window.setInterval(this.loadCardsFromServer, this.props.pollInterval);
    }
    
    render(){
       return (
            <div>
                <GameStatusForPlayer 
                            gameState={this.state.gameStatusForPlayer} 
                            gameOver = {this.state.gameOver}
                            toggleCardSelected={this.toggleSelectedVisibleCard} 
                            playCards={this.playCards} 
                            errors = {this.state.errors}
                            cannotPlayCards={this.cannotPlayCards} />
                            
                <GameStatusForOpponents
                            gameState={this.state.gameStatusForOpponents}
                            currentPlayer={this.state.gameStatusForPlayer.CurrentPlayer}
                            />
                            
                <GameRules rules={this.state.rules} />   
            </div> 
        );
    }
}

export default Game;

ReactDOM.render(
  <Game palaceConfig={PalaceConfig} pollInterval={2000} />,
  document.getElementById("reactContent")
);