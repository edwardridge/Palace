using System;

namespace Palace
{
	public enum ResultOutcome
	{
		Fail = 1,
		Success = 2
	}

    public class Result
    {
        private readonly ResultOutcome resultOutcome;

        public Result(ResultOutcome resultOutcome)
        {
            this.resultOutcome = resultOutcome;
        }

        public ResultOutcome ResultOutcome
        {
            get
            {
                return this.resultOutcome;
            }
        }
    }
}

