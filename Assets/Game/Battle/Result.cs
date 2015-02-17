namespace Choanji.Battle
{
	public enum ResultType
	{
		WIN,
		LOSE,
	}

	public class Result
	{
		public readonly ResultType type;

		public Result(ResultType _type)
		{
			type = _type;
		}

		public static implicit operator ResultType(Result _this)
		{
			return _this.type;
		}
	}

}