var GameStatusForOpponents = React.createClass({
    
    render: function(){
        var opponents = [];
        this.props.gameState.forEach(function(opponent, index){
            opponents.push(
                <div key={index}>
                    <GameStatusForOpponent 
                        cardsFaceUp={opponent.CardsFaceUp} 
                        cardsFaceDownNum={opponent.CardsFaceDownNum}
                        cardsInHandNum={opponent.CardsInHandNum}
                        name = {opponent.Name}
                    />
                </div>
                );
        })
        return(
            <div> {opponents} </div>
        );
}
});