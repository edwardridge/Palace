namespace Palace
{
    public class Card
    {
        public int Value { get; private set; }

		public Suit Suit { get; private set; }

		public CardType Type { get; private set; }

		public CardOrientation CardOrientation { get; set; }

		public Card(int value, CardType type, Suit suit) : this(value, suit, type, CardOrientation.InHand)
		{
		}

		public Card(int value, Suit suit, CardType type, CardOrientation faceUp)
        {
            this.Value = value;
			this.Suit = suit;
			this.Type = type;
			this.CardOrientation = faceUp;
        }

		public virtual bool Equals(Card comparison){
			return this.Suit == comparison.Suit && this.Value == comparison.Value;
		}
    }
}
