var VisibleCardPile = React.createClass({
    render: function () {
        var emptyFunction = function () { };

        return (
         <span>
             {this.props.cards.map(function (card, index){
                 return (
                   <span key={index} onClick={this.props.toggleCardSelected ? this.props.toggleCardSelected.bind(null, index) : emptyFunction}>
                        <Card cardVal={card} index={index} />
                   </span>
          );
        }, this)}

      </span>
    );
}});