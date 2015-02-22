using System.Collections;
using System.Collections.Generic;
using Gem;
using LitJson;

namespace Choanji
{
	public class Party : IEnumerable<Pair<Card, CardMode>>
	{
		public int count { get { return passives.Count + actives.Count; } }
		public bool isFull { get { return count >= Const.PARTY_MAX; } }

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

		public List<Card> GetCardsOf(CardMode _mode)
		{
			switch (_mode)
			{
				case CardMode.PASSIVE:
					return passives;
				case CardMode.ACTIVE:
					return actives;
			}

			return null;
		}

		public bool Add(Card _card, CardMode _mode)
		{
			Remove(_card);

			if (isFull) 
				return false;

			var _cards = GetCardsOf(_mode);

			if (!_cards.Contains(_card))
			{
				_cards.Add(_card);
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool Remove(Card _card)
		{
			if (passives.Remove(_card))
				return true;
			
			if (actives.Count == 1)
				return false;

			return actives.Remove(_card);
		}

		public IEnumerator<Pair<Card, CardMode>> GetEnumerator()
		{
			foreach (var _card in actives)
				yield return new Pair<Card, CardMode>(_card, CardMode.ACTIVE);
			foreach (var _card in passives)
				yield return new Pair<Card, CardMode>(_card, CardMode.PASSIVE);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

}