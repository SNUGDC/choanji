using System.Collections.Generic;
using Gem;

namespace Choanji.Battle
{
	public class AIAgent : Agent
	{
		private readonly Battler mBattler;

		public AIAgent(Battler _battler)
			: base(AgentType.AI)
		{
			mBattler = _battler;
		}

		protected override void DoStartCardSelect()
		{
			// todo: 개선
			var _card = mBattler.party.actives.Rand();
			EndCardSelect(new CardSelectYield(new List<Card>{ _card }));
		}
	}
}