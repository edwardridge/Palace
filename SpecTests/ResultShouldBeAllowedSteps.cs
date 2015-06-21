using System;
using TechTalk.SpecFlow;

namespace SpecTests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Palace;

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
