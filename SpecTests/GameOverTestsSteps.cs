using System;
using TechTalk.SpecFlow;

namespace SpecTests
{
    using FluentAssertions;

    using Palace;

    [Binding]
    public class GameOverTestsSteps
    {
        private Result result;
        public GameOverTestsSteps()
        {
            result = ScenarioContext.Current.Get<Result>();   
        }

        [Then(@"the game is over")]
        public void ThenTheGameIsOver()
        {
            result.ResultOutcome.Should().Be(ResultOutcome.GameOver);
        }

        [Then(@"you are notified the game is over")]
        public void ThenYouAreNotifiedTheGameIsOver()
        {
            result.ResultOutcome.Should().Be(ResultOutcome.GameOver);
        }


        [Then(@"'(.*)' wins the game")]
        public void ThenWinsTheGame(string p0)
        {
            
        }
    }
}
