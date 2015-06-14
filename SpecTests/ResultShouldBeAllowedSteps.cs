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
        private ResultWrapper resultWrapper;

        public ResultShouldBeAllowedSteps(ResultWrapper resultWrapper)
        {
            this.resultWrapper = resultWrapper;
        }


        [Then(@"this should not be allowed")]
        public void ThenThisShouldNotBeAllowed()
        {
            resultWrapper.resultOutcome.Should().Be(ResultOutcome.Fail);
        }

        [Then(@"this should be allowed")]
        public void ThenThisShouldBeAllowed()
        {
            resultWrapper.resultOutcome.Should().Be(ResultOutcome.Success);
        }

    }
}
