using System;
using TechTalk.SpecFlow;

namespace SpecTests
{
    using FluentAssertions;

    using Palace;

    [Binding]
    public class WhoShouldStartSteps
    {
        [When(@"The game starts")]
        public void WhenTheGameStarts()
        {
            var dealer = ScenarioContext.Current.Get<Dealer>("dealer");
            var game = dealer.StartGame();
            ScenarioContext.Current.Remove("game");
            ScenarioContext.Current.Add("game", game);
        }
        
        [Then(@"it should be '(.*)' turn")]
        public void ThenItShouldBeTurn(string p0)
        {
            var game = ScenarioContext.Current.Get<Game>("game");
            game.CurrentPlayer.Name.Should().Be(p0);
        }
    }
}
