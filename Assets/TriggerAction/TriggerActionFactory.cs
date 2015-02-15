using Gem;
using LitJson;

namespace Choanji
{
	public static class TriggerActionFactory 
	{
		public static TriggerAction Make(LocalCoor _coor, JsonData _data)
		{
			JsonData _triggerData;

			// check trigger and action type is valid.
			var _triggerType = TriggerType.INSPECT;

			if (_data.TryGet("trigger", out _triggerData))
			{
				if (!EnumHelper.TryParse((string)_triggerData["type"], out _triggerType))
					return null;
			}

			var _actionData = _data["action"];
			ActionType _actionType;
			if (!EnumHelper.TryParse((string)_actionData["type"], out _actionType))
				return null;

			// make trigger and action
			var _trigger = TriggerFactory.Make(_triggerType, _coor, _triggerData);
			var _action = ActionFactory.Make(_actionType, _actionData);

			if (_trigger == null || _action == null)
				return null;

			var _id = default(TriggerActionID);
			JsonData _idData;
			if (_data.TryGet("mode", out _idData))
				_id = TriggerActionHelper.MakeID((string)_idData);

			var _mode = TriggerActionMode.NORM;
			JsonData _modeData;
			if (_data.TryGet("mode", out _modeData))
				_mode = EnumHelper.ParseOrDefault<TriggerActionMode>((string)_modeData);

			return new TriggerAction(_id, _trigger, _action, _mode);
		}
	}
}