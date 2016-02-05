class GameStatusForPlayer extends React.Component{
    render() {
        var gameOver = this.props.gameOver;
        var gameOverMessage = this.props.gameOver ? <h2>GAME OVER! The winner is {this.props.gameState.CurrentPlayer}</h2> : null;
        var isPlayersTurn = this.props.gameState.CurrentPlayer === this.props.gameState.Name;
        
        var noSelectedCards = true;
        this.props.gameState.CardsInHand.forEach(function(card){
            if(card.selected) {noSelectedCards=false};
        });   

        var noFaceUpSelectedCards = true;
        this.props.gameState.CardsFaceUp.forEach(function(card){
            if(card.selected) {noFaceUpSelectedCards=false};
        });
        
        var fewerThanThreeCardsInHand = this.props.gameState.CardsInHand.length < 3;
        
        var otherPlayerText = 'It is ' + this.props.gameState.CurrentPlayer + '\'s go';
        var playersTurn = <div>{isPlayersTurn ? 'It is your go' : otherPlayerText}</div>
        
        var emptyFunction = function () { };
        
        var faceUpCardButton = null;
        if(fewerThanThreeCardsInHand){
            faceUpCardButton = <button onClick={this.props.playCards.bind(null, 'CardsFaceUp' )} disabled={noFaceUpSelectedCards || !isPlayersTurn || gameOver}>Play cards</button>;
        } 
        
        var isPlayersTurnClass = isPlayersTurn ? 'playersTurn row' : 'opponentsTurn row';
        
        return (
        <div>
            <div>{gameOverMessage}</div>
            <div className='errors'>{this.props.errors}</div>
            <h2>{playersTurn}</h2>
            <h2>{this.props.gameState.Name} (you)</h2>
            <span className={isPlayersTurnClass}>
                <span className='col-xs-12 col-sm-6 col-md-6'>
                    Cards in hand <br />
                    <VisibleCardPile cards={this.props.gameState.CardsInHand} toggleCardSelected={this.props.toggleCardSelected.bind(null, 'CardsInHand')} /> <br />
                    <button onClick={this.props.playCards.bind(null, 'CardsInHand')} disabled={noSelectedCards || !isPlayersTurn || gameOver}>Play cards</button> <br /> <br/>
                    <CannotPlay cannotPlayCards={this.props.cannotPlayCards} allowed={!isPlayersTurn || gameOver}/> <br/> <br/>
                </span>
                <span className='col-xs-12 col-sm-6 col-md-6'>
                    <span className='col-xs-6 col-sm-6 col-md-6'>
                        Face up cards <br />
                        <VisibleCardPile 
                            cards={this.props.gameState.CardsFaceUp} 
                            toggleCardSelected={this.props.toggleCardSelected.bind(null, 'CardsFaceUp')} />
                        {faceUpCardButton}
                    </span>
                    <span className='col-xs-6 col-sm-6 col-md-6'>
                        Face down cards <br/>
                        <FaceDownCards cardsCount={this.props.gameState.CardsFaceDownNum} playCards={(isPlayersTurn && !gameOver) ? this.props.playCards.bind(null, 'CardsFaceDown') : emptyFunction}/> <br/>
                    </span>
                </span>
            </span>
            <h2>Play pile</h2>
            <span className='row playPile'>
                <span className='col-xs-12 col-sm-12 col-md-12'>
                    
                    <VisibleCardPile cards={this.props.gameState.PlayPile} /> 
                </span>
            </span>
        </div>
      );
    }
};