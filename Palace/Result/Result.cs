
namespace Palace
{
	public class Result
	{
		public ResultOutcome Outcome {
			get;
			private set;
		}

		public Result(ResultOutcome resultOutcome){
			this.Outcome = resultOutcome;
		}
	}

}

