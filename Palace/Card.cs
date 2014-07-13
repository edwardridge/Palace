namespace Palace
{
    public class Card
    {
        public int Value { get; private set; }

		public bool FaceUp { get; set; }

		public Card(int value, bool faceUp)
        {
            Value = value;
			FaceUp = faceUp;
        }
    }
}
