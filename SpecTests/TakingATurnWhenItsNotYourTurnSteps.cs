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
    public class TakingATurnWhenItsNotYourTurnSteps
    {
        private ResultWrapper resultWrapper;

        public TakingATurnWhenItsNotYourTurnSteps(ResultWrapper resultWrapper)
        {
            this.resultWrapper = resultWrapper;
        }


        [Then(@"this should not be allowed")]
        public void ThenThisShouldNotBeAllowed()
        {
            resultWrapper.resultOutcome.Should().Be(ResultOutcome.Fail);
        }
    }
}
