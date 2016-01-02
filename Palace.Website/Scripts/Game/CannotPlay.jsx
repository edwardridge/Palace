var CannotPlay = React.createClass({

    render: function(){
        return (
        <button className="CannotPlay" onClick={this.props.cannotPlayCards} disabled={this.props.allowed}>
           I can not play a card!
    </button>
);
}
});