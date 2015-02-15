using Gem;
using LitJson;

namespace Choanji
{
	public static class ActionFactory 
	{
		public static Action_ Make(ActionType _type, JsonData _data)
		{
			switch (_type)
			{
				case ActionType.OPEN_DIALOG:
					return new ActionOpenDialog(_data);
				default:
					D.Assert(false);
					return null;
			}
		}
	}

}