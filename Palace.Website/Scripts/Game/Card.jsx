class Card extends React.Component{
    render(){
        var textToNumber = function(text){
            switch(text){
                case 'one': return 1;
                case 'two': return 2;
                case 'three': return 3;
                case 'four': return 4;
                case 'five': return 5;
                case 'six': return 6;
                case 'seven': return 7;
                case 'eight': return 8;
                case 'nine': return 9;
                case 'ten': return 10;

                default: return text;
            }
        }
        var imageName = '/Content/cards/' + textToNumber(this.props.cardVal.Value.toLowerCase()) + '_of_' + this.props.cardVal.Suit.toLowerCase() + 's.png';
        
        var imageClass = 'cardImage ';
        if(this.props.cardVal.selected){
            imageClass = imageClass + 'selected';
        }else{
            imageClass = imageClass + 'not-selected';
        }
        
        return (
            <span className='card'>
                <img src={imageName} className={imageClass} />
            </span>
        );
    }
}