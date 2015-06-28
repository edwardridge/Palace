using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palace
{
    public class Result
    {
        private List<string> _errorMessages;

        public Result()
        {
            this._errorMessages = new List<string>();
        }

        public Result(string error)
        {
            this._errorMessages = new List<string>(new[] { error });
        }

        public void AddErrorMessage(string message)
        {
            this._errorMessages.Add(message);
        }

        public virtual ResultOutcome ResultOutcome
        {
            get
            {
                return this._errorMessages.Any() ? ResultOutcome.Fail : ResultOutcome.Success;
            }
        }
    }
}
