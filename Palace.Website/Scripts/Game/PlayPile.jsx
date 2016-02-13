import VisibleCardPile from "./VisibleCardPile.jsx";

class PlayPile extends React.Component{
    render(){
        return (
            <div>
                <h2>Play pile</h2>
                <span className="row playPile">
                    <span className="col-xs-12 col-sm-12 col-md-12">
                        <VisibleCardPile cards={this.props.playPile} /> 
                    </span>
                </span>
            </div>
        );
    }
}

export default PlayPile;