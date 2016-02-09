import React from "react";

class CannotPlay extends React.Component {  
    render(){
        //console.log('func: ' + this.props.cannotPlayCards);
        return (
            <button className="CannotPlay" 
                onClick={this.props.cannotPlayCards} 
                disabled={this.props.disabled}>
            I can not play a card!
            </button>
        );
    }
}
//disabled={this.props.allowed}
export default CannotPlay;