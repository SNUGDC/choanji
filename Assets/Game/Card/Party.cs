using System.Collections.Generic;

namespace Choanji
{
	public class Party
	{
		public readonly List<Card> passives = new List<Card>();
		public readonly List<Card> actives = new List<Card>();

		public StatSet CalStat()
		{
			var _statSet = new StatSet();
			foreach (var _passive in passives)
				_statSet += _passive.data.stat;
			return _statSet;
		}
	}

}