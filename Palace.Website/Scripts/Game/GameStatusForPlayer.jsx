import VisibleCardPile from "./VisibleCardPile.jsx";
import FaceDownCards from "./FaceDownCards.jsx";
import CannotPlay from "./CannotPlay.jsx";

class GameStatusForPlayer extends React.Component{
    render() {
        let gameOver = this.props.gameOver;
        let gameOverMessage = this.props.gameOver ? <h2>GAME OVER! The winner is {this.props.gameState.CurrentPlayer}</h2> : null;
        let isPlayersTurn = this.props.gameState.CurrentPlayer === this.props.gameState.Name;
        
        let cardIsNotSelected = function(card){
            return !card.selected;
        };
        
        let noSelectedCards = this.props.gameState.CardsInHand.every(cardIsNotSelected);   

        let noFaceUpSelectedCards = this.props.gameState.CardsFaceUp.every(cardIsNotSelected);
        
        let fewerThanThreeCardsInHand = this.props.gameState.CardsInHand.length < 3;
        
        let otherPlayerText = `It is ${this.props.gameState.CurrentPlayer}s turn`;
        let playersTurn = <div>{isPlayersTurn ? "It is your go" : otherPlayerText}</div>;
        
        let emptyFunction = function () { };
        
        let faceUpCardButton = null;
        if(fewerThanThreeCardsInHand){
            faceUpCardButton = <button onClick={this.props.playCards.bind(null, "CardsFaceUp" )} disabled={noFaceUpSelectedCards || !isPlayersTurn || gameOver}>Play cards</button>;
        } 
        
        let isPlayersTurnClass = isPlayersTurn ? "playersTurn row" : "opponentsTurn row";
        
        return (
        <div>
            <div>{gameOverMessage}</div>
            <div className="errors">{this.props.errors}</div>
            <h2>{playersTurn}</h2>
            <h2>{this.props.gameState.Name} (you)</h2>
            <span className={isPlayersTurnClass}>
                <span className="col-xs-12 col-sm-6 col-md-6">
                    Cards in hand <br />
                    <VisibleCardPile cards={this.props.gameState.CardsInHand} toggleCardSelected={this.props.toggleCardSelected.bind(null, "CardsInHand")} /> <br />
                    <button onClick={this.props.playCards.bind(null, "CardsInHand")} disabled={noSelectedCards || !isPlayersTurn || gameOver}>Play cards</button> <br /> 
                    <CannotPlay cannotPlayCards={this.props.cannotPlayCards} disabled={!isPlayersTurn || gameOver}/> <br/> 
                </span>
                <span className="col-xs-12 col-sm-6 col-md-6">
                    <span className="col-xs-6 col-sm-6 col-md-6">
                        Face up cards <br />
                        <VisibleCardPile 
                            cards={this.props.gameState.CardsFaceUp} 
                            toggleCardSelected={this.props.toggleCardSelected.bind(null, "CardsFaceUp")} />
                        {faceUpCardButton}
                    </span>
                    <span className="col-xs-6 col-sm-6 col-md-6">
                        Face down cards <br/>
                        <FaceDownCards cardsCount={this.props.gameState.CardsFaceDownNum} playCards={(isPlayersTurn && !gameOver) ? this.props.playCards.bind(null, "CardsFaceDown") : emptyFunction}/> <br/>
                    </span>
                </span>
            </span>
        </div>
      );
    }
}

export default GameStatusForPlayer;