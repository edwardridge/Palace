var CannotPlay = React.createClass({

 render: function(){
    return (
    <button className="CannotPlay" onClick={this.props.cannotPlayCards}>
        I can't play a card!
    </button>
);
}
});

var Card = React.createClass({
 render: function(){
    var textToNumber = function(text){
        switch(text){
            case 'one': return 1;
            case 'two': return 2;
            case 'three': return 3;
            case 'four': return 4;
            case 'five': return 5;
            case 'six': return 6;
            case 'seven': return 7;
            case 'eight': return 8;
            case 'nine': return 9;
            case 'ten': return 10;

            default: return text;
        }
    }
    var imageName = '/Content/cards/' + textToNumber(this.props.cardVal.Value.toLowerCase()) + '_of_' + this.props.cardVal.Suit.toLowerCase() + 's.png';
    return (

        <img src={imageName} width='100px' height='144px' className={this.props.cardVal.selected ? 'selected' :
             'not-selected' } />

);
}
});

var FaceDownCards = React.createClass({
    
    render: function(){
        var faceDown = [];
        for(var i = 0; i < this.props.cardsCount; i++){
            faceDown.push(<span key={i} ><img src='/Content/cards/card_back.png' width='100px' height='144px' /></span>)
        }
        return(
            <div className='FaceDown'>
            {faceDown}
            </div>
        );
    }
});

var VisibleCardPile = React.createClass({
    render:function() {
     return (
      <span className="Test">
          {this.props.cards.map(function (card, index){
          return (
            <span key={index} onClick={this.props.toggleCardSelected.bind(null, index)}>
                <Card cardVal={card} index={index} />
            </span>
          );
          }, this)}

      </span>
    );
}});

var GameStatus = React.createClass({
  render: function() {
    var noSelectedCards = true;
    this.props.gameState.CardsInHand.forEach(function(card){
        if(card.selected) {noSelectedCards=false};
    });   

    var noFaceUpSelectedCards = true;
    this.props.gameState.CardsFaceUp.forEach(function(card){
        if(card.selected) {noFaceUpSelectedCards=false};
    });  
    
    return (
    <div>
        <div className='errors'>{this.props.error}</div>
        <h3>Name: {this.props.gameState.Name} </h3> <br />
        {this.props.gameState.CurrentPlayer === this.props.gameState.Name ? 'It is your go' : 'It is not your go'} <br />
    
        Play pile: <br />
        <VisibleCardPile cards={this.props.gameState.PlayPile} toggleCardSelected={this.props.toggleCardSelected} /> <br />
     
        Cards in hand: <br />
        <VisibleCardPile cards={this.props.gameState.CardsInHand} toggleCardSelected={this.props.toggleCardSelected.bind(null, 'CardsInHand')} /> <br />
        <button onClick={this.props.playCards.bind(null, 'CardsInHand')} disabled={noSelectedCards}>Play cards</button> <br /> <br/>
        <CannotPlay cannotPlayCards={this.props.cannotPlayCards} /> <br/> <br/>
        Face down cards <br/>
        <FaceDownCards cardsCount={this.props.gameState.CardsFaceDownNum}/> <br/>
        Face up cards: <br />
        <VisibleCardPile cards={this.props.gameState.CardsFaceUp} toggleCardSelected={this.props.toggleCardSelected.bind(null, 'CardsFaceUp')} /> <br />
        <button onClick={this.props.playCards.bind(null, 'CardsFaceUp' )} disabled={noFaceUpSelectedCards}>Play cards</button> <br /> <br />
        </div>
    );
  }
});

var Game = React.createClass({
getInitialState: function() {
        return {
            data: {CardsInHand: [],CardsFaceUp: [], PlayPile: [], NumberOfValidMoves: -1},
            errors: []
        };
      },
    toggleSelectedOfFaceUpCard: function(cardPile, index){
        this.state.data[cardPile][index].selected = !this.state.data[cardPile][index].selected;
        this.setState({data: this.state.data });
    },
    playCards: function(cardPile){
        var game = this;
        var cardsToPlay = [];
        this.state.data[cardPile].forEach(function(card){
            if(card.selected){
                cardsToPlay.push(card);
            }
        });
        var methodToSend = cardPile === 'CardInHand' ? window.playInHandCard : window.playFaceUpCard ;
        this.sendPostRequestAndUpdateState(game, methodToSend, JSON.stringify(cardsToPlay));
    },
    cannotPlayCards: function(event){
        var game = this;
        this.sendPostRequestAndUpdateState(game, window.cannotPlayCard);
    },
    sendPostRequestAndUpdateState: function(game, url, postData){
        var xhr = new XMLHttpRequest();
        xhr.open('post', url,  true);
        xhr.setRequestHeader("Content-type", "application/json");
        xhr.onload = function() {
            game.updateStateFromResult(game, xhr.responseText, true);
        };
        xhr.send(postData);
    },
    loadCommentsFromServer: function() {
        var game = this;
        var xhr = new XMLHttpRequest();
        xhr.open('get', game.props.url, true);
        xhr.onload = function() {
            game.updateStateFromResult(game, xhr.responseText, false);
        };
        xhr.send();
   },
    updateStateFromResult: function(game, responseText, forceUpdate){
        var data = JSON.parse(responseText);
        if(forceUpdate || game.state.data.length === 0 || data.GameStatusForPlayer.NumberOfValidMoves > game.state.data.NumberOfValidMoves){
            game.setState({ data: data.GameStatusForPlayer, errors: data.Errors});
        }
    },
    componentDidMount: function() {
        this.loadCommentsFromServer();
        window.setInterval(this.loadCommentsFromServer, this.props.pollInterval);
    },
   render: function(){
       var error = this.state.errors[0];
    return (
        <GameStatus 
                    gameState={this.state.data} 
                    toggleCardSelected={this.toggleSelectedOfFaceUpCard} 
                    playCards={this.playCards} 
                    error = {error}
                    cannotPlayCards={this.cannotPlayCards} />
    )
    }
});

ReactDOM.render(

  <Game url={window.getUrl} pollInterval={2000} />,
  document.getElementById('reactContent')
);