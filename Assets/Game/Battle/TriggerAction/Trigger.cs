using LitJson;

namespace Choanji.Battle
{
	public class Trigger
	{
		public readonly TriggerWhen when;
		public readonly TriggerWhere where;
		public readonly int? prob;

		public Trigger()
		{}

		public Trigger(JsonData _data)
		{
			JsonData _when;
			if (_data.TryGet("when", out _when))
				when = TriggerFactory.MakeWhen(_when);

			JsonData _where;
			if (_data.TryGet("where", out _where))
				where = TriggerFactory.MakeWhere(_where);

			JsonData _prob;
			if (_data.TryGet("prob", out _prob))
				prob = (int)_prob;
		}
	}
}