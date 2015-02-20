using System;
using System.Collections.Generic;
using Gem;

namespace Choanji.Battle
{
	public class TAManager
	{
		private class TAInfo
		{
			public readonly TA ta;
			public readonly Battler battler;
			public Action cleanup;

			public TAInfo(TA _ta, Battler _battler)
			{
				ta = _ta;
				battler = _battler;
			}
		}

		private readonly List<TAInfo> mTAs = new List<TAInfo>();

		~TAManager()
		{
			Clear();
		}

		public void Add(Battler _battler, TA _ta)
		{
			var _taInfo = new TAInfo(_ta, _battler);

			if (_ta.trigger == null)
			{
				var _event = TheBattle.onStart;
				Action _fire = () => Fire(_battler, _ta.action);
				_event += _fire;
				_taInfo.cleanup = () => { _event -= _fire; };
				mTAs.Add(_taInfo);
				return;
			}

			var _triggerWhen = _ta.trigger.when;
			var _when = _triggerWhen != null
				? _triggerWhen.type : TriggerWhenType.BATTLE_START;

			switch (_when)
			{
				case TriggerWhenType.BATTLE_START:
					{
						Action _fire = () => TestAndFire(_battler, _ta, null);
						TheBattle.onStart += _fire;
						_taInfo.cleanup = () => { TheBattle.onStart -= _fire; };
						break;
					}

				case TriggerWhenType.BEFORE_HIT:
					{
						var _target = TheBattle.state.GetStateOf(_battler);
						Action<Damage> _fire = _dmg => TestAndFire(_battler, _ta, _dmg);
						_target.beforeHit += _fire;
						_taInfo.cleanup = () => { _target.beforeHit -= _fire; };
						break;
					}

				default:
					throw new ArgumentOutOfRangeException();
			}

			mTAs.Add(_taInfo);
		}

		public void Remove(Battler _battler, TA _ta)
		{
			var _taInfo = mTAs.FindAndRemoveIf(e => (e.battler == _battler) && (e.ta == _ta));
			_taInfo.cleanup();
		}

		public void Clear()
		{
			foreach (var _taInfo in mTAs)
				_taInfo.cleanup();
			mTAs.Clear();
		}

		private static bool Test(TriggerWhere _where, object _arg)
		{
			switch (_where.type)
			{
				case TriggerWhereType.TRUE:
					return true;
				case TriggerWhereType.CUR_DMG_TYPE:
					return ((TriggerWhereEleType)_where).Test(((Damage)_arg).ele);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static bool Test(int _prob)
		{
			return UnityEngine.Random.Range(0, 100) < _prob;
		}

		private static void Fire(Battler _battler, Action_ _action)
		{
			switch (_action.type)
			{
				case ActionType.AVOID_HIT:
					TheBattle.state.GetStateOf(_battler).blockHitOneTime = true;
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static bool TestAndFire(Battler _battler, TA _ta, object _arg)
		{
			if (_ta.trigger.where != null
				&& !Test(_ta.trigger.where, _arg))
			{
				return false;
			}

			if (_ta.trigger.prob.HasValue
				&& !Test(_ta.trigger.prob.Value))
			{
				return false;
			}

			Fire(_battler, _ta.action);

			return true;
		}
	}
}