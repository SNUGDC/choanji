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
				passives.Add(new Card(_card));
			foreach (var _card in _data["active"].GetListEnum())
				actives.Add(new Card(_card));
		}

		public JsonData Serialize()
		{
			var _data = new JsonData();

			var _passive = _data["passive"] = new JsonData();
			foreach (var _card in passives)
				_passive.Add(_card.Serialize());

			var _active = _data["active"] = new JsonData();
			foreach (var _card in actives)
				_active.Add(_card.Serialize());

			return _data;
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