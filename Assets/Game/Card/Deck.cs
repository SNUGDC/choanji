using System.Collections.Generic;
using Gem;
using LitJson;

namespace Choanji
{
	public class Deck
	{
		public readonly List<Card> cards = new List<Card>();

		public Deck()
		{}

		public Deck(JsonData _data)
		{
			foreach (var _cardData in _data.GetListEnum())
				cards.Add(new Card(_cardData));
		}

		public JsonData Serialize()
		{
			var _data = new JsonData();
			_data.SetJsonType(JsonType.Array);
			foreach (var _card in cards)
				_data.Add(_card.Serialize());
			return _data;
		}
	}
}
