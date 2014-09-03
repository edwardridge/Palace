using System;
using NUnit.Framework;
using Palace;
using FluentAssertions;

namespace UnitTests
{
	[TestFixture]
	public class GameInProgressTest
	{
		[Test]
		public void Player_Is_Set_To_Ready_On_Setup ()
		{
			var player1 = new Player ("Ed");
			var deck = new Deck (new NonShuffler());
			var game = new Game ();
			game.Setup (new []{ player1 }, deck);

			player1.State.Should ().Be (PlayerState.Ready);
		}
	}
}

