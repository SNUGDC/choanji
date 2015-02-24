using System;
using System.Collections.Generic;

namespace Choanji.Battle
{
	public class MyAgent : Agent
	{
		public MyAgent() 
			: base(AgentType.ME)
		{}

		protected override void DoStartCardSelect()
		{
			Action<List<Card>> _callback = _cards => EndCardSelect(new CardSelectYield(_cards));
			TheBattle.digest.Enq(new TypedDigest(null, DigestType.CARD_SELECT, _callback));
		}
	}
}