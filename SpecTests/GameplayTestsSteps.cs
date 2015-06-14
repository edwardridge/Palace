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
        [Then(@"the number of cards in the play pile should be '(.*)'")]
        public void ThenTheNumberOfCardsInThePlayPileShouldBe(int p0)
        {
            var game = ScenarioContext.Current.Get<Game>("game");
            game.PlayPileCardCount.Should().Be(p0);
        }

        [Then(@"'(.*)' should have '(.*)' cards in hand")]
        public void ThenShouldHaveCardsInHand(string p0, int p1)
        {
            var game = ScenarioContext.Current.Get<Game>("game");
            game.Players.First(p => p.Name.Equals(p0)).NumCardsInHand.Should().Be(3);
        }

    }


}
