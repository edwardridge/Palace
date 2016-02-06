import Card from "./Card.jsx";

class VisibleCardPile extends React.Component{
    render() {
        let emptyFunction = function () { };

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
    }
}

export default VisibleCardPile;