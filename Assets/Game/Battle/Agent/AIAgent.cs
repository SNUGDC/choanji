using System.Collections.Generic;
using System.Linq;
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
			var _available = mBattler.ap;
			var _cards = new List<Card>();

			var _actives = new List<Card>(mBattler.party.actives);
			_actives.Shuffle();

			while (!_actives.Empty())
			{
				var _card = _actives.Last();
				var _cost = _card.data.active.cost;
				if (_cost > _available) break;
				_available -= _cost;
				_actives.RemoveBack();
				_cards.Add(_card);
			}

			EndCardSelect(new CardSelectYield(_cards));
		}
	}
}