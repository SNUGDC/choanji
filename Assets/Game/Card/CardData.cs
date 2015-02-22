using Gem;
using LitJson;

namespace Choanji
{
	public enum CardMode
	{
		PASSIVE,
		ACTIVE,
	}

	public class CardData
	{
		private CardData(string _key)
		{
			id = CardHelper.MakeID(_key);
			key = _key;	
		}

		public CardData(string _key, JsonData _data)
			: this(_key)
		{
			ele = ElementDB.Search(_data.StringOrDefault("ele", "NOR"));
			name = (string)_data["name"];
			detail = (string) _data["detail"];
			stat = new StatSet(_data["stat"]);
			passive = MoveDB.Get(CardHelper.MakePassiveID((string)_data["passive"]));
			active = MoveDB.Get(CardHelper.MakeActiveID((string)_data["active"]));
		}

		public readonly CardID id;
		public readonly string key;
		public readonly ElementID ele;
		public readonly string name;
		public readonly string detail;
		public readonly StatSet stat;
		public readonly PassiveData passive;
		public readonly ActiveData active;

		public static implicit operator CardID(CardData _this)
		{
			return _this.id;
		}
	}

}