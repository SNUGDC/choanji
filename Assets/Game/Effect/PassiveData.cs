using Gem;
using LitJson;

namespace Choanji
{
	public class PassiveData
	{
		PassiveData(string _key, string _name, string _detail)
		{
			id = (PassiveID)HashEnsure.Do(_key);
			key = _key;
			name = _name;
			detail = _detail;
		}

		public PassiveData(JsonData _data)
			: this((string)_data["key"], (string)_data["name"], (string)_data["detail"])
		{}

		public readonly PassiveID id;
		public readonly string key;
		public readonly string name;
		public readonly string detail;
	}

}