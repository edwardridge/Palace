using System;
using TechTalk.SpecFlow;

namespace SpecTests
{
    using FluentAssertions;

    using Palace;

    [Binding]
    public class ItShouldBePlayersTurnSteps
    {
        private Game game;
        
        public ItShouldBePlayersTurnSteps()
        {
            game = ScenarioContext.Current.Get<Game>();
        }

        [Then(@"it should be '(.*)' turn")]
        public void ThenItShouldBeTurn(string p0)
        {
            game.State.CurrentPlayer.Name.Should().Be(p0);
        }
    }
}
