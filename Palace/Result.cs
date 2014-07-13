
namespace Palace
{
	public class Result
	{
		public ResultOutcome ResultOutcome {
			get;
			private set;
		}

		public Result(ResultOutcome resultOutcome){
			this.ResultOutcome = resultOutcome;
		}
	}

}

