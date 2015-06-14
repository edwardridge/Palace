using System;
using TechTalk.SpecFlow;

namespace SpecTests
{
    using FluentAssertions;

    using Palace;

    [Binding]
    public class WhoShouldStartSteps
    {   
        [Then(@"it should be '(.*)' turn")]
        public void ThenItShouldBeTurn(string p0)
        {
            var game = ScenarioContext.Current.Get<Game>("game");
            game.CurrentPlayer.Name.Should().Be(p0);
        }
    }
}
