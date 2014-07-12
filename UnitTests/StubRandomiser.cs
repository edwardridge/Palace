using System;
using Palace;

namespace UnitTests
{
	public class StubRandomiser : IRandomiser
	{
		public StubRandomiser ()
		{
		}

		public int GetRandom (int max)
		{
			return 0; //Always return the first card
		}
	}
}

