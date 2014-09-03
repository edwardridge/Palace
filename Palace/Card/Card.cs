namespace Palace
{
    public class Card
    {
		public Suit Suit { get; private set; }

		public CardValue Value { get; private set; }

		public CardOrientation CardOrientation { get; set; }

		public Card(CardValue value, Suit suit) : this(value, suit, CardOrientation.InHand)
		{
		}

		public Card(CardValue value, Suit suit,  CardOrientation cardOrientation)
        {
			this.Suit = suit;
			this.Value = value;
			this.CardOrientation = cardOrientation;
        }

		public virtual bool Equals(Card comparison){
			return this.Suit == comparison.Suit && this.Value == comparison.Value;
		}
    }
}
