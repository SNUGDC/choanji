using Gem;
using LitJson;

namespace Choanji
{
	public class PassiveData
	{
		public PassiveData(string _key, string _name, string _detail)
		{
			id = (PassiveID)HashEnsure.Do(_key);
			key = _key;
			name = _name;
			detail = _detail;
		}

		public PassiveData(string _key, JsonData _data)
			: this(_key, (string)_data["name"], (string)_data["detail"])
		{}

		public readonly PassiveID id;
		public readonly string key;
		public readonly string name;
		public readonly string detail;

		public static implicit operator PassiveID(PassiveData _this)
		{
			return _this.id;
		}
	}

}