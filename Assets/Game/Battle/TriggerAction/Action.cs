using Gem;
using LitJson;

namespace Choanji.Battle
{
	public enum ActionType
	{
		NONE = 0,
		AVOID_HIT,
	}

	public class Action_
	{
		public readonly ActionType type;

		public Action_(JsonData _data)
		{
			type = EnumHelper.ParseOrDefault<ActionType>((string)_data["type"]);
		}
	}
}