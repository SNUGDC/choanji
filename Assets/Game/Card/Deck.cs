using System.Collections.Generic;
using Gem;
using LitJson;

namespace Choanji
{
	public class Deck
	{
		public readonly Dictionary<CardID, Card> cards = new Dictionary<CardID, Card>();

		public Deck()
		{}

		public Deck(JsonData _data)
		{
			foreach (var _cardData in _data.GetListEnum())
			{
				var _card = new Card(_cardData);
				cards.Add(_card.data, _card);	
			}
		}

		public JsonData Serialize()
		{
			var _data = new JsonData();
			_data.SetJsonType(JsonType.Array);
			foreach (var _card in cards)
				_data.Add(_card.Value.Serialize());
			return _data;
		}

		public Card Find(CardID _id)
		{
			return cards.GetOrDefault(_id);
		}
	}
}
