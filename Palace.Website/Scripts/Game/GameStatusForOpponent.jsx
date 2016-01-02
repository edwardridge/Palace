var GameStatusForOpponent = React.createClass({
    
    render: function(){
        
        return(
            <div>
                Game status for {this.props.name} <br/>
                Face up cards: <br />
                <VisibleCardPile cards={this.props.cardsFaceUp} /> <br />
                
                Face down cards: <br />
                <FaceDownCards cardsCount={this.props.cardsFaceDownNum}/> <br/>
                
                In hand cards: <br />
                <FaceDownCards cardsCount={this.props.cardsInHandNum}/> <br/>
            </div>
        );
}
});