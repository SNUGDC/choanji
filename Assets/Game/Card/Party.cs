using System.Collections.Generic;

namespace Choanji
{
	public class Party
	{
		public readonly List<CardData> passives = new List<CardData>();
		public readonly List<CardData> actives = new List<CardData>();

		public StatSet CalStat()
		{
			var _statSet = new StatSet();
			foreach (var _passive in passives)
				_statSet += _passive.stat;
			return _statSet;
		}
	}

}