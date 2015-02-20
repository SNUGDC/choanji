using Choanji.ActivePerform;
using UnityEngine;

namespace Choanji.Battle
{
	public class PerformResult
	{}

	public class PerformDmgResult : PerformResult
	{
		public bool hit;
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
				var _dmgTrue = Other(_battler).Hit(_perform.dmg);
				return new PerformDmgResult {
					hit = true, 
					dmg = new Damage(_perform.dmg.ele, _dmgTrue)
				};
			}
			else
			{
				return new PerformDmgResult
				{
					hit = false
				};
			}
		}
	}
}