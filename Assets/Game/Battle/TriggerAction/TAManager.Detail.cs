using System;
using System.Collections.Generic;
using Gem;
using Random = UnityEngine.Random;

namespace Choanji.Battle
{
	public partial class TAManager
	{
		private readonly Dictionary<TA, Action> mMap = new Dictionary<TA, Action>();

		private void Enable(Battler _battler, TA _ta)
		{
			Action _cleanup;

			switch (_ta.trigger.when.type)
			{
				case TriggerWhenType.BATTLE_START:
				{
					var _event = TheBattle.onStart;
					Action _fire = () => TestAndFire(_battler, _ta, null);
					_event += _fire;
					_cleanup = () => { _event -= _fire; };
					break;	
				}

				case TriggerWhenType.BEFORE_HIT:
				{
					var _event = TheBattle.state.GetStateOf(_battler).beforeHit;
					Action<Damage> _fire = _dmg => TestAndFire(_battler, _ta, _dmg);
					_event += _fire;
					_cleanup = () => { _event -= _fire; };
					break;
				}

				default:
					throw new ArgumentOutOfRangeException();
			}

			mMap.Add(_ta, _cleanup);
		}

		private void Disable(TA _ta)
		{
			Action _cleanup;
			if (mMap.GetAndRemove(_ta, out _cleanup))
				_cleanup();
		}

		private static bool Test(TriggerWhere _where)
		{
			switch (_where.type)
			{
				case TriggerWhereType.TRUE:
					return true;
				case TriggerWhereType.CUR_DMG_TYPE:
					// todo: implement
					return false;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static bool Test(int _prob)
		{
			return Random.Range(0, 100) < _prob;
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

		private bool TestAndFire(Battler _battler, TA _ta, object _arg)
		{
			if (_ta.trigger.where != null
			    && !Test(_ta.trigger.where))
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