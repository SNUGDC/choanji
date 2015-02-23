using System;
using System.Collections.Generic;
using Gem;

namespace Choanji.Battle
{
	public class TriggerManager
	{
		class TAInfo
		{
			public readonly Invoker invoker;
			public readonly TA ta;

			public Trigger trigger { get { return ta.trigger; } }
			public Action_ action { get { return ta.action; } }
			public Action cleanup;

			public int count;

			public TAInfo(Invoker _invoker, TA _ta)
			{
				invoker = _invoker;
				ta = _ta;
			}
		}

		private readonly List<TAInfo> mTAs = new List<TAInfo>();

		~TriggerManager()
		{
			Clear();
		}

		private static Battler Choose(TargetType _target, Battler _self, Battler _other)
		{
			return _target == TargetType.SELF ? _self : _other;
		}

		public void Add(Invoker _invoker, TA _ta)
		{
			if (_ta.trigger == null)
			{
				L.E("trigger should not be null.");
				return;
			}

			var _state = TheBattle.state;
			var _self = _invoker.battler;
			var _other = _state.Other(_invoker.battler);

			var _taInfo = new TAInfo(_invoker, _ta);
			var _trigger = _taInfo.trigger;

			var _triggerWhen = _trigger.when;
			var _whenType = _triggerWhen != null
				? _triggerWhen.type : TriggerWhenType.BATTLE_START;

			switch (_whenType)
			{
				case TriggerWhenType.BATTLE_START:
				{
					Action _fire = () => TestAndFire(_taInfo, null);
					TheBattle.onStart += _fire;
					_taInfo.cleanup = () => { TheBattle.onStart -= _fire; };
					break;
				}

				case TriggerWhenType.BEFORE_START_TURN:
				{
					Action _fire = () => TestAndFire(_taInfo, null);
					TheBattle.battle.onTurnStart += _fire;
					_taInfo.cleanup = () => { TheBattle.battle.onTurnStart -= _fire; };
					break;
				}

				case TriggerWhenType.BEFORE_HIT:
				{
					Action<Damage> _fire = _dmg => TestAndFire(_taInfo, _dmg);
					_self.beforeHit += _fire;
					_taInfo.cleanup = () => { _self.beforeHit -= _fire; };
					break;
				}

				case TriggerWhenType.AFTER_HIT:
				{
					var _when = (TriggerWhenTarget)_trigger.when;
					var _target = Choose(_when.target, _self, _other);
					Action<Damage> _fire = _dmg => TestAndFire(_taInfo, _dmg);
					_target.afterHit += _fire;
					_taInfo.cleanup = () => { _target.afterHit -= _fire; };
					break;
				}

				default:
					L.E("when " + _whenType + " is not implemented.");
					return;
			}

			mTAs.Add(_taInfo);
		}

		private void Remove(TAInfo _ta)
		{
			mTAs.Remove(_ta);
			_ta.cleanup();
		}

		public void Clear()
		{
			foreach (var _taInfo in mTAs)
				_taInfo.cleanup();
			mTAs.Clear();
		}

		private static bool Test(Trigger _trigger, object _arg)
		{
			var _where = _trigger.where;

			if (_where == null)
				return true;

			switch (_where.type)
			{
				case TriggerWhereType.TRUE:
					return true;
				case TriggerWhereType.CUR_DMG_TYPE:
					return ((TriggerWhereEleType)_where).Test(((Damage)_arg).ele);
				default:
					L.E("where " + _where.type + " is not implemented.");
					return false;
			}
		}

		private Digest TestAndFire(TAInfo _taInfo, object _arg)
		{
			if (!Test(_taInfo.trigger, _arg))
				return null;

			++_taInfo.count;

			var _limit = _taInfo.trigger.limit;

			if (_limit.HasValue)
			{
				if (_taInfo.count >= _limit)
					Remove(_taInfo);
			}

			return Fire(_taInfo.invoker, _taInfo.action, _arg);
		}

		public Digest Fire(Invoker _invoker, Action_ _action, object _arg)
		{
			var _result = _action.Invoke(_invoker, _arg);
			if (_result != null) TheBattle.digest.Enq(_result);
			return _result;
		}
	}
}