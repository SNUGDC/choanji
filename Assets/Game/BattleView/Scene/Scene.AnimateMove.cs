
using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	public partial class Scene
	{
		public float AnimateActive(Battler _battler, ActiveData _active)
		{
			var _isA = TheBattle.state.IsA(_battler);
			if (_isA)
				return AnimateActiveA(_battler, _active);
			else
				return AnimateActiveB(_battler, _active);
		}

		public float AnimateActiveA(Battler _battler, ActiveData _active)
		{
			var a = _battler;
			var b = TheBattle.state.battlerB;

			var base_ = field.base_.transform;
			var center = field.enemyCenter;

			GameObject _fx = null;

			switch (_active.key)
			{
				case "SMASH":
				{
					_fx = FXDB.g.smash.Instantiate();
					break;
				}

				case "SHARP_TONGUED":
				{
					_fx = FXDB.g.sharpTongued.Instantiate();
					break;
				}

				case "BASIC_VOCA_ATTACK":
				{
					_fx = FXDB.g.basicVocaAttack.Instantiate();
					break;
				}

				case "BLOCK_TRAP":
				{
					_fx = FXDB.g.blockTrap.Instantiate();
					break;
				}

				case "GROWLING":
				{
					_fx = FXDB.g.growling.Instantiate();
					break;
				}

				case "NUCLEAR_508":
				{
					// note: 한턴 후에 적용.
					// _fx = FXDB.g.nuclear508.Instantiate();
					break;
				}

				case "ROCK_AND_ROLL":
				{
					_fx = FXDB.g.rockAndRoll.Instantiate();
					break;
				}
			}

			if (_fx)
			{
				_fx.transform.SetParent(center.transform, false);
				_fx.transform.Translate(Vector3.forward);				
			}

			return 0;
		}

		public float AnimateActiveB(Battler _battler, ActiveData _active)
		{
			var a = TheBattle.state.battlerA;
			var b = _battler;

			var base_ = field.base_.transform;
			var center = field.enemyCenter;

			GameObject _fx = null;

			switch (_active.key)
			{
				case "GUARD_UP":
				{
					_fx = FXDB.g.guardUp.Instantiate();
					break;
				}

				case "FRYING_FAN_UP":
				{
					_fx = FXDB.g.fryingFanUp.Instantiate();
					break;
				}

				case "LICK":
				{
					// note: 세턴간 적용.
					break;
				}

				case "FIRST_AID_KIT":
				{
					break;
				}
			}

			if (_fx)
			{
				_fx.transform.SetParent(center.transform, false);
				_fx.transform.Translate(Vector3.forward);
			}

			return 0;
		}
	}
}