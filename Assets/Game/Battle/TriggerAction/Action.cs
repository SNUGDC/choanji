using Gem;
using LitJson;

namespace Choanji.Battle
{
	public enum ActionType
	{
		NONE = 0,
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

	public static class ActionFactory
	{
		public static Action_ Make(JsonData _data)
		{
			var _type = EnumHelper.ParseOrDefault<ActionType>((string)_data["type"]);

			switch (_type)
			{
				case ActionType.BUFF_ATK:
				case ActionType.BUFF_DEF:
					return new ActionBuffEle(_type, _data);
				default:
					return new Action_(_type);
			}
		}
	}
}