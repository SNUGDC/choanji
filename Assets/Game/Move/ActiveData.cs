using Gem;
using LitJson;

namespace Choanji
{
	public class ActiveData
	{
		public ActiveData(string _key, JsonData _data)
		{
			id = (ActiveID)HashEnsure.Do(_key);
			key = _key;
			type = EnumHelper.ParseOrDefault<ActiveType>((string) _data["type"]);
			name = (string) _data["name"];
			detail = (string) _data["detail"];
			cost = (AP) (int) _data["cost"];
			perform = ActivePerform.Factory.Make(_data["perform"]);
		}

		public readonly ActiveID id;
		public readonly string key;
		public readonly ActiveType type;
		public readonly string name;
		public readonly string detail;

		public readonly AP cost;

		public readonly ActivePerform.Base perform;

		public static implicit operator ActiveID(ActiveData _this)
		{
			return _this.id;
		}
	}
}