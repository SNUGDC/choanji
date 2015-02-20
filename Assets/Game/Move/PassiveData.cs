using Gem;
using LitJson;

namespace Choanji
{
	public class PassiveData
	{
		public PassiveData(string _key, JsonData _data)
		{
			id = (PassiveID)HashEnsure.Do(_key);
			key = _key;
			name = (string) _data["name"];
			detail = (string) _data["detail"];
			perform = new Battle.TA(_data["perform"]);
		}

		public readonly PassiveID id;
		public readonly string key;
		public readonly string name;
		public readonly string detail;
		public readonly Battle.TA perform;

		public static implicit operator PassiveID(PassiveData _this)
		{
			return _this.id;
		}
	}

}