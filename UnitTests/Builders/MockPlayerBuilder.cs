﻿using System;
using Palace;
using Moq;
using System.Collections.Generic;

namespace UnitTests
{
	public class MockPlayerBuilder
	{
		Mock<IPlayer> mockPlayer;

		public MockPlayerBuilder(){
			mockPlayer = new Mock<IPlayer> ();
		}

		public MockPlayerBuilder WithName(string name){
			mockPlayer.SetupGet(prop => prop.Name).Returns(name);
			return this;
		}

		public MockPlayerBuilder WithState(PlayerState state){
			mockPlayer.SetupGet(prop => prop.State).Returns(state);
			return this;
		}

		public MockPlayerBuilder WithCards(ICollection<int> cards){
			mockPlayer.SetupGet (prop => prop.Cards).Returns(CardHelpers.GetCardsFromValues (cards));
			return this;
		}

		public IPlayer Build(){
			return this.mockPlayer.Object;
		}

		public static MockPlayerBuilder MockReadyPlayer(){
			var player = new MockPlayerBuilder().WithState (PlayerState.Ready);
			return player;
		}
	}
}

