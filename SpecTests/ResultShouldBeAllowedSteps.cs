namespace SpecTests
{
    using FluentAssertions;

    using Palace;

    using TechTalk.SpecFlow;

    [Binding]
    public class ResultShouldBeAllowedSteps
    {
        private Result result;

        public ResultShouldBeAllowedSteps()
        {
            result = ScenarioContext.Current.Get<Result>();
        }

        [Then(@"this should not be allowed")]
        public void ThenThisShouldNotBeAllowed()
        {
            result.ResultOutcome.Should().Be(ResultOutcome.Fail);
        }

        [Then(@"this should be allowed")]
        public void ThenThisShouldBeAllowed()
        {
            result.ResultOutcome.Should().Be(ResultOutcome.Success);
        }
    }
}