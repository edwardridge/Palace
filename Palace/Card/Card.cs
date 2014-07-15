namespace Palace
{
    public class Card
    {
        public int Value { get; private set; }

		public Suite Suite { get; private set; }

		public CardOrientation CardOrientation { get; set; }

		public Card(int value, Suite suite, CardOrientation faceUp)
        {
            Value = value;
			Suite = suite;
			CardOrientation = faceUp;
        }
    }
}
