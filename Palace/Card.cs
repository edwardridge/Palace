namespace Palace
{
    public class Card
    {
        public int Value { get; private set; }

		public FaceOrientation FaceOrientation { get; set; }

		public Card(int value, FaceOrientation faceUp)
        {
            Value = value;
			FaceOrientation = faceUp;
        }
    }
}
