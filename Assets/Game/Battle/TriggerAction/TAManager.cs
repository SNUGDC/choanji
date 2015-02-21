﻿using System;
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

		private class DelayedFire
		{
			public readonly int start;
			public int schedule { get { return start + ta.delay; } }

			public readonly Battler battler;
			public readonly TA ta;
			public readonly object arg;
			
			public DelayedFire(int _start, Battler _battler, TA _ta, object _arg)
			{
				start = _start;
				battler = _battler;
				ta = _ta;
				arg = _arg;
			}
		}

		private int mCurTurn;
		private readonly List<DelayedFire> mDelayedFires = new List<DelayedFire>();

		~TAManager()
		{
			Clear();
		}

		public void Add(Battler _battler, TA _ta)
		{
			var _taInfo = new TAInfo(_ta, _battler);

			if (_ta.trigger == null)
			{
				Action _fire = () => Fire(_battler, _ta.action);
				TheBattle.onStart += _fire;
				_taInfo.cleanup = () => { TheBattle.onStart -= _fire; };
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
						Action<Damage> _fire = _dmg => TestAndFire(_battler, _ta, _dmg);
						_battler.beforeHit += _fire;
						_taInfo.cleanup = () => { _battler.beforeHit -= _fire; };
						break;
					}

				default:
					L.E("when " + _when + " is not implemented.");
					return;
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
					L.E("where " + _where.type + " is not implemented.");
					return false;
			}
		}

		private static bool Test(int _prob)
		{
			if (_prob >= 100) return true;
			return UnityEngine.Random.Range(0, 100) < _prob;
		}
		
		public IEnumerable<ActionResult> FireDelayed()
		{
			++mCurTurn;

			mDelayedFires.Sort((a, b) => a.schedule.CompareTo(b.schedule));

			var i = 0;
			foreach (var _delayedFire in mDelayedFires)
			{
				if (_delayedFire.schedule >= mCurTurn)
				{
					yield return Fire(_delayedFire.battler, _delayedFire.ta.action);
					++i;
				}
				else
				{
					break;
				}
			}

			mDelayedFires.RemoveRange(0, i);
		}

		public static ActionResult Fire(Battler _battler, Action_ _action)
		{
			switch (_action.type)
			{
				case ActionType.DMG:
					return Fire(_battler, (ActionDmg)_action);

				case ActionType.AVOID_HIT:
					_battler.blockHitOneTime = true;
					break;

				case ActionType.STAT_MOD:
					_battler.dynamicStat += ((ActionStatMod)_action).stat;
					break;

				case ActionType.BUFF_ATK:
				{
					var _theAction = (ActionBuffEle)_action;
					_battler.attackModifier[_theAction.ele] += _theAction.per;
					break;
				}

				default:
					L.E("action " + _action.type + " is not implemented.");
					break;
			}

			return null;
		}

		private static ActionResult Fire(Battler _battler, ActionDmg _action)
		{
			if (Test(_action.accuracy))
			{
				L.D("hit");

				var _state = TheBattle.state;

				var _dmg = _battler.attackBuilder.Build(_action.dmg);

				var _hitter = _state.Other(_battler);
				_hitter.beforeHit.CheckAndCall(_dmg);

				if (_hitter.blockHitOneTime)
				{
					L.D("block");
					_hitter.blockHitOneTime = false;
					return new ActionDmgResult
					{
						hit = true,
						block = true,
					};
				}
				else
				{
					var _dmgTrue = _hitter.Hit(_dmg);
					return new ActionDmgResult
					{
						hit = true,
						dmg = new Damage(_dmg.ele, _dmgTrue)
					};
				}
			}
			else
			{
				L.D("miss");
				return new ActionDmgResult
				{
					hit = false
				};
			}
		}

		public ActionResult TestAndFire(Battler _battler, TA _ta, object _arg)
		{
			if (_ta.delay == 0)
			{
				return DoTestAndFire(_battler, _ta, _arg);
			}
			else
			{
				if (DoTest(_ta.trigger, _arg))
					mDelayedFires.Add(new DelayedFire(mCurTurn, _battler, _ta, _arg));
				return new ActionDelayedResult();
			}
		}

		private static bool DoTest(Trigger _trigger, object _arg)
		{
			if (_trigger == null)
				return true;

			if (_trigger.where != null
				&& !Test(_trigger.where, _arg))
			{
				return false;
			}

			if (_trigger.prob.HasValue
				&& !Test(_trigger.prob.Value))
			{
				return false;
			}

			return true;
		}

		private static ActionResult DoTestAndFire(Battler _battler, TA _ta, object _arg)
		{
			if (!DoTest(_ta.trigger, _arg))
				return null;
			return Fire(_battler, _ta.action);
		}
	}
}