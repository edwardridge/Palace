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
        [Then(@"this should not be allowed")]
        public void ThenThisShouldNotBeAllowed()
        {
            var result = ScenarioContext.Current.Get<Result>("result");
            result.ResultOutcome.Should().Be(ResultOutcome.Fail);
        }

        [Then(@"this should be allowed")]
        public void ThenThisShouldBeAllowed()
        {
            var result = ScenarioContext.Current.Get<Result>("result");
            result.ResultOutcome.Should().Be(ResultOutcome.Success);
        }

    }
}
