﻿var FaceDownCards = React.createClass({
    
    render: function(){
        var faceDown = [];
        var emptyFunction = function() {};
        for(var i = 0; i < this.props.cardsCount; i++){
            faceDown.push(
                <span key={i} 
                onClick={this.props.playCards ? this.props.playCards : emptyFunction}>
                    <img src='/Content/cards/card_back.png' width='75px' height='110px' />
                </span>
                )
    }
        return(
            <span className='FaceDown'>
            {faceDown}
            </span>
        );
}
});