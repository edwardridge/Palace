namespace Palace
{
    public class Card
    {
		public Suit Suit { get; private set; }

		public CardValue Value { get; private set; }

		public CardOrientation CardOrientation { get; set; }

		public CardType CardType { get; private set; }

		public Card(CardValue value, Suit suit) : this(value, suit, CardOrientation.InHand)
		{
		}

		public Card(CardValue value, Suit suit,  CardOrientation cardOrientation) 
			:this(value, suit, cardOrientation, CardType.Standard){
		}

		public Card(CardValue value, Suit suit, CardType type) 
			:this(value, suit, CardOrientation.InHand, type){
		}

		public Card(CardValue value, Suit suit,  CardOrientation cardOrientation, CardType type)
        {
			this.Suit = suit;
			this.Value = value;
			this.CardOrientation = cardOrientation;
			this.CardType = type;
        }

		public virtual bool Equals(Card comparison){
			return this.Suit == comparison.Suit && this.Value == comparison.Value;
		}
    }
}
