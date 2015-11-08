var CannotPlay = React.createClass({

 render: function(){
    return (
    <a className="CannotPlay" onClick={this.props.cannotPlayCards}>
        I can't play a card!
    </a>
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
    
    return (
      <div className="Test">
          <div className='errors'>{this.props.error}</div>
          <h3>Name: {this.props.gameState.Name} </h3> <br />
          {this.props.gameState.CurrentPlayer === this.props.gameState.Name ? 'It is your go' : 'It is not your go'} <br />
          Num face down cards: {this.props.gameState.CardsFaceDownNum} <br />
          Cards in hand: <br />
          <VisibleCardPile cards={this.props.gameState.CardsInHand} toggleCardSelected={this.props.toggleCardSelected} /> <br />
          <button onClick={this.props.playCards} disabled={noSelectedCards}>Play cards</button> <br />
          Face up cards: <br />
          <VisibleCardPile cards={this.props.gameState.CardsFaceUp} toggleCardSelected={this.props.toggleCardSelected} /> <br />
          Play pile: <br />
          <VisibleCardPile cards={this.props.gameState.PlayPile} toggleCardSelected={this.props.toggleCardSelected} /> <br />

          <CannotPlay cannotPlayCards={this.props.cannotPlayCards} />
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
    toggleSelectedOfFaceUpCard: function(index){
        this.state.data.CardsInHand[index].selected = !this.state.data.CardsInHand[index].selected;
        this.setState({data: this.state.data });

    },
    playCards: function(){
        var game = this;
        var cardsToPlay = [];
        this.state.data.CardsInHand.forEach(function(card){
            if(card.selected){
                cardsToPlay.push(card);
            }
        });

        var xhr = new XMLHttpRequest();
        xhr.open('post', window.playInHandCard, true);
        xhr.onload = function() {
            game.updateStateFromResult(game, xhr.responseText, true);
        };
        xhr.setRequestHeader("Content-type", "application/json");
        xhr.send(JSON.stringify(cardsToPlay));
    },
    cannotPlayCards: function(event){
        var game = this;
        this.sendPostRequestAndUpdateState(game, window.cannotPlayCard);
    },
    sendPostRequestAndUpdateState: function(game, url){
        var xhr = new XMLHttpRequest();
        xhr.open('post', url,  true);
        xhr.setRequestHeader("Content-type", "application/json");
        xhr.onload = function() {
            game.updateStateFromResult(game, xhr.responseText, true);
        };
        xhr.send();
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
                    cannotPlayCards={this.cannotPlayCards} 
         />
    )
    }
});

ReactDOM.render(

  <Game url={window.getUrl} pollInterval={2000} />,
  document.getElementById('reactContent')
);