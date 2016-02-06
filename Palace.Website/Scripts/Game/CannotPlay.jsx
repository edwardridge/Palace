class CannotPlay extends React.Component {  
    render(){
        return (
            <button className="CannotPlay" onClick={this.props.cannotPlayCards} disabled={this.props.allowed}>
            I can not play a card!
            </button>
        );
    }
}

export default CannotPlay