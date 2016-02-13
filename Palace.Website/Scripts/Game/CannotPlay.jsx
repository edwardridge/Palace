import React from "react";

class CannotPlay extends React.Component {  
    render(){
        return (
            <button className="CannotPlay" 
                onClick={this.props.cannotPlayCards} 
                disabled={this.props.disabled}>
            I can not play a card!
            </button>
        );
    }
}

export default CannotPlay;