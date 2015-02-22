using LitJson;

namespace Choanji.Battle
{
	public class Trigger
	{
		public readonly TriggerWhen when;
		public readonly TriggerWhere where;
		public readonly int? limit;

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

			JsonData _limit;
			if (_data.TryGet("limit", out _limit))
				limit = (int)_limit;
		}
	}

	public struct TA
	{
		public readonly Trigger trigger;
		public readonly Action_ action;

		public TA(Trigger _trigger, Action_ _action)
		{
			trigger = _trigger;
			action = _action;
		}

		public TA(JsonData _data)
		{
			JsonData _triggerData;
			trigger = _data.TryGet("trigger", out _triggerData) 
				? new Trigger(_triggerData) : null;
			action = ActionFactory.Make(_data["action"]);
		}
	}
}