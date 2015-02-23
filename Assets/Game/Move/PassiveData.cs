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
			theme = _data.StringOrDefault("theme", "0000ff");
			perform = new Battle.TA(_data["perform"]);
		}

		public readonly PassiveID id;
		public readonly string key;
		public readonly string name;
		public readonly string detail;
		public readonly string theme;
		public readonly Battle.TA perform;

		public string richName { get { return UIHelper.RichAddColor(name, theme); } }

		public static implicit operator PassiveID(PassiveData _this)
		{
			return _this.id;
		}
	}

}