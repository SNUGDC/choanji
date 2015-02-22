using LitJson;

namespace Choanji
{
	public class Card
	{
		public readonly CardData data;

		public Card(CardData _data)
		{
			data = _data;
		}

		public Card(JsonData _data)
		{
			var _key = _data.IsString 
				? (string) _data : (string) _data["key"];
			data = CardDB.Get(CardHelper.MakeID(_key));
		}

		public JsonData Serialize()
		{
			var _data = new JsonData();
			_data["key"] = data.key;
			return _data;
		}
	}
}