using System;
using TechTalk.SpecFlow;

namespace SpecTests
{
    using FluentAssertions;

    using Palace;

    [Binding]
    public class GameOverTestsSteps
    {

        [Then(@"the game is over")]
        public void ThenTheGameIsOver()
        {
            var result = ScenarioContext.Current.Get<Result>("result");
            result.ResultOutcome.Should().Be(ResultOutcome.GameOver);
        }

        [Then(@"you are notified the game is over")]
        public void ThenYouAreNotifiedTheGameIsOver()
        {
            var result = ScenarioContext.Current.Get<Result>("result");
            result.ResultOutcome.Should().Be(ResultOutcome.GameOver);
        }


        [Then(@"'(.*)' wins the game")]
        public void ThenWinsTheGame(string p0)
        {
            
        }
    }
}
