namespace Palace
{
    public class Card
    {
        //public int Value { get; private set; }

		public Suit Suit { get; private set; }

		public CardType Type { get; private set; }

		public CardOrientation CardOrientation { get; set; }

		public Card(CardType type, Suit suit) : this(type, suit, CardOrientation.InHand)
		{
		}

		public Card(CardType type, Suit suit,  CardOrientation cardOrientation)
        {
			this.Suit = suit;
			this.Type = type;
			this.CardOrientation = cardOrientation;
        }

		public virtual bool Equals(Card comparison){
			return this.Suit == comparison.Suit && this.Type == comparison.Type;
		}
    }
}
