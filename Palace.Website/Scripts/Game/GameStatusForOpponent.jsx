var GameStatusForOpponent = React.createClass({
    
    render: function(){
        var isPlayersTurn = this.props.name === this.props.currentPlayer;
        var isPlayersTurnClass = isPlayersTurn ? 'playersTurn row' : 'opponentsTurn row';
        return(
            <span>
                <h2>{this.props.name}</h2>
                <span className={isPlayersTurnClass}>
                    
                    <span className='col-xs-12 col-sm-6 col-md-6'>
                        In hand cards: <br />
                        <FaceDownCards cardsCount={this.props.cardsInHandNum}/> <br/>
                    </span>
                    <span className='col-xs-12 col-sm-6 col-md-6'>
                        <span className='col-xs-6 col-sm-6 col-md-6'>
                            Face up cards: <br />
                            <VisibleCardPile cards={this.props.cardsFaceUp} /> <br />
                        </span>
                        <span className='col-xs-6 col-sm-6 col-md-6'>
                            Face down cards: <br />
                            <FaceDownCards cardsCount={this.props.cardsFaceDownNum}/> <br/>
                        </span>
                    </span>
                </span>
            </span>
        );
}
});