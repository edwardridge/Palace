class FaceDownCards extends React.Component{
    render(){
        var faceDown = [];
        var emptyFunction = function() {};
        for(var i = 0; i < this.props.cardsCount; i++){
            faceDown.push(
                <span key={i} 
                onClick={this.props.playCards ? this.props.playCards : emptyFunction}>
                    <img src='/Content/cards/card_back.png' className='cardImage' />
                </span>
            )
        }
        return(
            <span className='FaceDown'>
            {faceDown}
            </span>
        );
    }
};