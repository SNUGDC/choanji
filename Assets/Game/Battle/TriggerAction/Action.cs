using Gem;
using LitJson;

namespace Choanji.Battle
{
	public enum ActionType
	{
		NONE = 0,
		DMG,
		AVOID_HIT,
		BUFF_ATK,
		BUFF_DEF,
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

	public class ActionResult
	{ }

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
				case ActionType.BUFF_ATK:
				case ActionType.BUFF_DEF:
					return new ActionBuffEle(_type, _data);
				default:
					return new Action_(_type);
			}
		}
	}
}