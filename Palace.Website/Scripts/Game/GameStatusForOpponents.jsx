class GameStatusForOpponents extends React.Component{
    render(){
        var opponents = [];
        var gameStatusForOpponents = this;
        this.props.gameState.forEach(function(opponent, index){
            opponents.push(
                <div key={index}>
                    <GameStatusForOpponent 
                        cardsFaceUp={opponent.CardsFaceUp} 
                        cardsFaceDownNum={opponent.CardsFaceDownNum}
                        cardsInHandNum={opponent.CardsInHandNum}
                        name = {opponent.Name}
                        currentPlayer = {gameStatusForOpponents.props.currentPlayer}
                    />
                </div>
                );
        })
        return(
            <div> {opponents} </div>
        );
    }   
};