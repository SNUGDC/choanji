using System.Collections.Generic;
using Gem;
using LitJson;

namespace Choanji
{
	public class Party
	{
		public readonly List<Card> passives = new List<Card>();
		public readonly List<Card> actives = new List<Card>();

		public Party()
		{}

		public Party(JsonData _data)
		{
			foreach (var _card in _data["passive"].GetListEnum())
				passives.Add(new Card(CardDB.Get(CardHelper.MakeID((string)_card))));
			foreach (var _card in _data["active"].GetListEnum())
				actives.Add(new Card(CardDB.Get(CardHelper.MakeID((string)_card))));
		}

		public StatSet CalStat()
		{
			var _statSet = new StatSet();
			foreach (var _passive in passives)
				_statSet += _passive.data.stat;
			return _statSet;
		}
	}

}