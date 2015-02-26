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
			JsonData _cards;

			if (_data.TryGet("passive", out _cards))
			{
				foreach (var _card in _cards.GetListEnum())
					passives.Add(new Card(_card));	
			}

			if (_data.TryGet("active", out _cards))
			{
				foreach (var _card in _cards.GetListEnum())
					actives.Add(new Card(_card));	
			}
		}

		public Party(Deck _deck, JsonData _data)
		{
			JsonData _cards;

			if (_data.TryGet("passive", out _cards))
			{
				foreach (var _key in _cards.GetListEnum())
				{
					var _card = _deck.Find(CardHelper.MakeID((string)_key));
					if (_card != null) passives.Add(_card);
				}
			}

			if (_data.TryGet("active", out _cards))
			{
				foreach (var _key in _cards.GetListEnum())
				{
					var _card = _deck.Find(CardHelper.MakeID((string)_key));
					if (_card != null) actives.Add(_card);
				}
			}
		}

		public JsonData Serialize()
		{
			var _data = new JsonData();

			if (!passives.Empty())
			{
				var _passive = _data["passive"] = new JsonData();
				foreach (var _card in passives)
					_passive.Add(_card.data.key);	
			}

			if (!actives.Empty())
			{
				var _active = _data["active"] = new JsonData();
				foreach (var _card in actives)
					_active.Add(_card.data.key);
			}

			return _data;
		}

		public StatSet CalStat()
		{
			var _statSet = new StatSet();
			foreach (var _card in passives)
				_statSet += _card.data.stat;
			foreach (var _card in actives)
				_statSet += _card.data.stat;
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