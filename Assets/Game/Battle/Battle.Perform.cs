using Choanji.ActivePerform;
using Gem;
using Random = UnityEngine.Random;

namespace Choanji.Battle
{
	public class PerformResult
	{}

	public class PerformDmgResult : PerformResult
	{
		public bool hit;
		public bool block;
		public Damage? dmg;
	}

	public partial class Battle
	{
		private static bool Dice(int _prob)
		{
			if (_prob >= 100) return true;
			return Random.Range(0, 100) < _prob;
		}

		private Battler Other(Battler _battler)
		{
			return (state.battlerA == _battler)
				? state.battlerB : state.battlerA;
		}

		private PerformResult PerformActive(Battler _battler, Card _card)
		{
			var _perform = _card.data.active.perform;

			switch (_perform.id)
			{
				case ID.DMG:
					return PerformDmg(_battler, (Dmg)_perform);
			}

			return null;
		}

		private PerformResult PerformDmg(Battler _battler, Dmg _perform)
		{
			if (Dice(_perform.accuracy))
			{
				L.D("hit");

				var _attacker = state.GetStateOf(_battler);
				var _dmg = _attacker.attackBuilder.Build(_perform.dmg);

				var _hitter = Other(_battler);
				var _hitterState = state.GetStateOf(_hitter);
				_hitterState.beforeHit.CheckAndCall(_dmg);

				if (_hitterState.blockHitOneTime)
				{
					L.D("block");
					_hitterState.blockHitOneTime = false;
					return new PerformDmgResult
					{
						hit = true,
						block = true,
					};
				}
				else
				{
					var _dmgTrue = _hitter.Hit(_dmg);
					return new PerformDmgResult
					{
						hit = true,
						dmg = new Damage(_dmg.ele, _dmgTrue)
					};	
				}
			}
			else
			{
				L.D("miss");
				return new PerformDmgResult
				{
					hit = false
				};
			}
		}
	}
}