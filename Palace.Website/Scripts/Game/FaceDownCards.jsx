class FaceDownCards extends React.Component{
    render(){
        let faceDown = [];
        let emptyFunction = function() {};
        for(let i = 0; i < this.props.cardsCount; i++){
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

export default FaceDownCards