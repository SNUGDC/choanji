using System;
using Gem;
using LitJson;

namespace Choanji
{
	public enum ActionType
	{
		OPEN_DIALOG,
		SAVE,
	}

	public abstract class Action_
	{
		public readonly ActionType type;

		public Action onDone;

		protected Action_(ActionType _type)
		{
			type = _type;
		}

		public abstract void Do(object _data);
	}

	public sealed class ActionSave : Action_
	{
		private Action_ mOnSuccess;
		private Action_ mOnFail;

		public ActionSave(JsonData _data)
			: base(ActionType.SAVE)
		{
			JsonData _val;

			if (_data.TryGet("success", out _val))
				mOnSuccess = ActionFactory.Make(_val);

			if (_data.TryGet("fail", out _val))
				mOnFail = ActionFactory.Make(_val);
		}

		public override void Do(object _data)
		{
			if (Disket.Save())
			{
				mOnSuccess.onDone = onDone;
				mOnSuccess.Do(null);
			}
			else
			{
				mOnFail.onDone = onDone;
				mOnFail.Do(null);
			}
		}
	}

	public static class ActionFactory
	{
		public static Action_ Make(JsonData _data)
		{
			ActionType _type;
			if (!EnumHelper.TryParse((string) _data["type"], out _type))
				return null;
			return Make(_type, _data);
		}

		public static Action_ Make(ActionType _type, JsonData _data)
		{
			switch (_type)
			{
				case ActionType.SAVE:
					return new ActionSave(_data);
				case ActionType.OPEN_DIALOG:
					return new ActionOpenDialog(_data);
				default:
					D.Assert(false);
					return null;
			}
		}
	}
}