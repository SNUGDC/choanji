using System;
using System.Collections.Generic;
using System.Linq;
using Choanji.Battle;
using Gem;
using LitJson;

namespace Choanji
{
	public enum ActionType
	{
		SEQUENCE,
		OPEN_DIALOG,
		SAVE,
		START_BATTLE,
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

	public sealed class ActionSequence : Action_
	{
		private readonly List<Action_> mSequence = new List<Action_>();

		public ActionSequence(JsonData _data)
			: base(ActionType.SAVE)
		{
			Action_ _actionPrev = null;
			
			foreach (var _actionData in _data["sequence"].GetListEnum())
			{
				var _actionCur = ActionFactory.Make(_actionData);
				if (_actionCur == null) 
					continue;
				if (_actionPrev != null)
					_actionPrev.onDone = () => _actionCur.Do(null);
				mSequence.Add(_actionCur);
				_actionPrev = _actionCur;
			}
		}

		public override void Do(object _data)
		{
			mSequence.Last().onDone = onDone.CheckAndCall;
			mSequence.First().Do(_data);
		}
	}

	public sealed class ActionSave : Action_
	{
		private readonly Action_ mOnSuccess;
		private readonly Action_ mOnFail;

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

	public sealed class ActionStartBattle : Action_
	{
		private readonly BattlerID mBattler;

		public ActionStartBattle(JsonData _data)
			: base(ActionType.START_BATTLE)
		{
			mBattler = BattlerHelper.MakeID((string)_data["battler"]);
		}

		public override void Do(object _data)
		{
			var _self = Player.MakeBattler();
			var _battler = BattlerDB.Get(mBattler);

			TheBattle.Setup(new Setup(Mode.PVE, _self, _battler));
			TheBattle.battle.onTurnEnd = TheBattle.battle.StartTurn;
			TheBattle.Start();

			onDone.CheckAndCall();
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
				case ActionType.SEQUENCE:
					return new ActionSequence(_data);
				case ActionType.OPEN_DIALOG:
					return new ActionOpenDialog(_data);
				case ActionType.SAVE:
					return new ActionSave(_data);
				case ActionType.START_BATTLE:
					return new ActionStartBattle(_data);
				default:
					D.Assert(false);
					return null;
			}
		}
	}
}