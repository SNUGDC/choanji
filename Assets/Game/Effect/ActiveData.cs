using Gem;
using LitJson;

namespace Choanji
{
	public class ActiveData
	{
		public ActiveData(string _key, string _name, string _detail, int _cost)
		{
			id = (ActiveID)HashEnsure.Do(_key);
			key = _key;
			name = _name;
			detail = _detail;
			cost = _cost;
		}

		public ActiveData(JsonData _data)
			: this((string)_data["key"], (string)_data["name"], (string)_data["detail"], (int)_data["cost"])
		{}

		public readonly ActiveID id;
		public readonly string key;
		public readonly string name;
		public readonly string detail;

		public readonly int cost;
	}
}