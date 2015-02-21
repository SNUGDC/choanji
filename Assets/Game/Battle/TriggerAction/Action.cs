using Gem;
using LitJson;

namespace Choanji.Battle
{
	public enum ActionType
	{
		NONE = 0,
		DMG,
		HEAL,
		AVOID_HIT,
		STAT_MOD,
		BUFF_ATK,
		SC_HEAL,
	}

	public enum TargetType
	{
		SELF, OTHER,
	}

	public class Action_
	{
		public readonly ActionType type;

		public Action_(ActionType _type)
		{
			type = _type;
		}

		public static implicit operator ActionType(Action_ _this)
		{
			return _this.type;
		}
	}

	public sealed class ActionDmg : Action_
	{
		public readonly Damage dmg;
		public readonly int accuracy;

		public ActionDmg(JsonData _data)
			: base(ActionType.DMG)
		{
			var _ele = ElementDB.Search((string)_data["ele"]);
			var _val = (HP)(int)_data["dmg"];
			dmg = new Damage(_ele, _val);
			accuracy = _data.IntOrDefault("accuracy", 100);
		}
	}

	public sealed class ActionHeal : Action_
	{
		public readonly HP? val;
		public readonly Percent? per;

		public ActionHeal(JsonData _data)
			: base(ActionType.HEAL)
		{
			JsonData _val;

			if (_data.TryGet("val", out _val))
				val = (HP)(int) _val;
			else if (_data.TryGet("per", out _val))
				per = (Percent)(int)_val;
			else
			{
				D.Assert(false);
				val = 0;
			}
		}
	}

	public sealed class ActionBuffEle : Action_
	{
		public readonly ElementID ele;
		public readonly int per;

		public ActionBuffEle(ActionType _type, JsonData _data)
			: base(_type)
		{
			ele = ElementDB.Search((string) _data["ele"]);
			per = (int) _data["per"];
		}
	}

	public sealed class ActionStatMod : Action_
	{
		public readonly int? dur;
		public readonly TargetType target;
		public readonly StatSet stat;

		public ActionStatMod(JsonData _data)
			: base(ActionType.STAT_MOD)
		{
			stat = new StatSet(_data);

			target = TargetType.SELF;
			JsonData _targetJs;
			if (_data.TryGet("target", out _targetJs))
				EnumHelper.TryParse((string) _targetJs, out target);

			JsonData _durJs;
			if (_data.TryGet("dur", out _durJs))
				dur = (int)_durJs;
		}
	}

	public sealed class ActionSCHeal : Action_
	{
		public readonly Percent accuracy;

		public ActionSCHeal(JsonData _data)
			: base(ActionType.SC_HEAL)
		{
			accuracy = (Percent)_data.IntOrDefault("accuracy", 100);
		}
	}

	public class ActionResult
	{ }

	public class ActionDelayedResult : ActionResult
	{}

	public class ActionDmgResult : ActionResult
	{
		public bool hit;
		public bool block;
		public Damage? dmg;
	}

	public static class ActionFactory
	{
		public static Action_ Make(JsonData _data)
		{
			if (_data.IsString)
				return new Action_(EnumHelper.ParseOrDefault<ActionType>((string)_data));

			var _type = EnumHelper.ParseOrDefault<ActionType>((string)_data["type"]);

			switch (_type)
			{
				case ActionType.DMG:
					return new ActionDmg(_data);
				case ActionType.HEAL:
					return new ActionHeal(_data);
				case ActionType.STAT_MOD:
					return new ActionStatMod(_data);
				case ActionType.BUFF_ATK:
					return new ActionBuffEle(_type, _data);
				case ActionType.SC_HEAL:
					return new ActionSCHeal(_data);
				default:
					return new Action_(_type);
			}
		}
	}
}