namespace Palace
{
    using System;

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

        public override int GetHashCode()
        {
            return (int)this.Value.GetHashCode() + (int)this.Suit.GetHashCode();
        }

		public override bool Equals(Object comparison)
		{
		    var comparisonCard = comparison as Card;
		    if (comparisonCard ==  null)
		        return false;
			return this.Suit == comparisonCard.Suit && this.Value == comparisonCard.Value;
		}

        public static Card AceOfClubs { get { return new Card(CardValue.Ace, Suit.Club); } }

        public static Card TwoOfClubs { get { return new Card(CardValue.Two, Suit.Club); } }

        public static Card ThreeOfClubs { get { return new Card(CardValue.Three, Suit.Club); } }
        
        public static Card FourOfClubs { get { return new Card(CardValue.Four, Suit.Club); } }

        public static Card FiveOfClubs { get { return new Card(CardValue.Five, Suit.Club); } }

        public static Card SixOfClubs { get { return new Card(CardValue.Six, Suit.Club); } }

        public static Card SevenOfClubs { get { return new Card(CardValue.Seven, Suit.Club); } }

        public static Card EightOfClubs { get { return new Card(CardValue.Eight, Suit.Club); } }

        public static Card FourOfSpades { get { return new Card(CardValue.Four, Suit.Spade); } }

        public static Card JackOfClubs { get
        {
            return new Card(CardValue.Jack, Suit.Spade);
        } }

        public static Card TenOfClubs
        {
            get
            {
                return new Card(CardValue.Ten, Suit.Club);
            }
        }
    }
}
