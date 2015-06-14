using System;

namespace Palace
{
    using System.Collections.Generic;
    using System.Linq;

    public enum ResultOutcome
	{
		Fail = 1,
		Success = 2
	}

    public class Result
    {
        private readonly ResultOutcome resultOutcome;

        private List<string> _errorMessages;

        public Result()
        {
            this._errorMessages = new List<string>();
        }

        public Result(string error)
        {
            this._errorMessages = new List<string>(new[]{error});
        }

        public void AddErrorMessage(string message)
        {
            this._errorMessages.Add(message);
        }

        public ResultOutcome ResultOutcome
        {
            get
            {
                return this._errorMessages.Any() ? ResultOutcome.Fail : ResultOutcome.Success;
            }
        }
    }
}

