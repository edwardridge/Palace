﻿using System;
using TechTalk.SpecFlow;

namespace SpecTests
{
    using System.Linq;

    using FluentAssertions;

    using Palace;

    [Binding]
    public class GameplayTestsSteps
    {
        private Game game;

        public GameplayTestsSteps()
        {
            game = ScenarioContext.Current.Get<Game>();   
        }

        [Then(@"the number of cards in the play pile should be '(.*)'")]
        public void ThenTheNumberOfCardsInThePlayPileShouldBe(int p0)
        {
            game.PlayPile.Count.Should().Be(p0);
        }

        [Then(@"'(.*)' should have '(.*)' cards in hand")]
        public void ThenShouldHaveCardsInHand(string playerName, int cardCount)
        {
            game.Players.First(p => p.Name.Equals(playerName)).NumCardsInHand.Should().Be(3);
        }

        [Then(@"'(.*)' should have '(.*)' cards face down")]
        public void ThenShouldHaveCardsFaceDown(string p0, int p1)
        {
            game.Players.First(p => p.Name.Equals(p0)).NumCardsFaceDown.Should().Be(p1);
        }

        [Then(@"'(.*)' should have '(.*)' cards face up")]
        public void ThenShouldHaveCardsFaceUp(string p0, int p1)
        {
            game.Players.First(p => p.Name.Equals(p0)).CardsFaceUp.Count.Should().Be(p1);
        }

    }


}
