var GameStatusForPlayer = React.createClass({
    render: function() {
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
        
        var otherPlayerText = 'It is ' + this.props.gameState.CurrentPlayer + '\'s go';
        var playersTurn = <div>{isPlayersTurn ? 'It is your go' : otherPlayerText}</div>
        
        var emptyFunction = function () { };
    
        return (
        <div>
            <div>{gameOverMessage}</div>
            <div className='errors'>{this.props.errors}</div>
            <h3>Name: {this.props.gameState.Name} </h3> <br />
            <span>{playersTurn}</span>
    
            Play pile: <br />
            <VisibleCardPile cards={this.props.gameState.PlayPile} /> <br />
     
            Cards in hand: <br />
            <VisibleCardPile cards={this.props.gameState.CardsInHand} toggleCardSelected={this.props.toggleCardSelected.bind(null, 'CardsInHand')} /> <br />
            <button onClick={this.props.playCards.bind(null, 'CardsInHand')} disabled={noSelectedCards || !isPlayersTurn || gameOver}>Play cards</button> <br /> <br/>
            <CannotPlay cannotPlayCards={this.props.cannotPlayCards} allowed={!isPlayersTurn || gameOver}/> <br/> <br/>
            Face down cards <br/>
            <FaceDownCards cardsCount={this.props.gameState.CardsFaceDownNum} playCards={(isPlayersTurn && !gameOver) ? this.props.playCards.bind(null, 'CardsFaceDown') : emptyFunction}/> <br/>
            Face up cards: <br />
            <VisibleCardPile cards={this.props.gameState.CardsFaceUp} toggleCardSelected={this.props.toggleCardSelected.bind(null, 'CardsFaceUp')} /> <br />
            <button onClick={this.props.playCards.bind(null, 'CardsFaceUp' )} disabled={noFaceUpSelectedCards || !isPlayersTurn || gameOver}>Play cards</button> <br /> <br />
            </div>
      );
    }
});