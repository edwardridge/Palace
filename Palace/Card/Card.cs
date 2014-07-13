namespace Palace
{
    public class Card
    {
        public int Value { get; private set; }

		public CardOrientation CardOrientation { get; set; }

		public Card(int value, CardOrientation faceUp)
        {
            Value = value;
			CardOrientation = faceUp;
        }
    }
}
