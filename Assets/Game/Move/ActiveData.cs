using Gem;
using LitJson;

namespace Choanji
{
	public class ActiveData
	{
		public ActiveData(string _key, ActiveType _type, string _name, string _detail, int _cost, ActivePerform.Base _perform)
		{
			id = (ActiveID)HashEnsure.Do(_key);
			key = _key;
			type = _type;
			name = _name;
			detail = _detail;
			cost = _cost;
			perform = _perform;
		}

		public ActiveData(string _key, JsonData _data)
			: this(
			_key,
			EnumHelper.ParseOrDefault<ActiveType>((string)_data["type"]),
			(string)_data["name"],
			(string)_data["detail"],
			(int)_data["cost"],
			ActivePerform.Factory.Make(_data["perform"]))
		{}

		public readonly ActiveID id;
		public readonly string key;
		public readonly ActiveType type;
		public readonly string name;
		public readonly string detail;

		public readonly int cost;

		public readonly ActivePerform.Base perform;

		public static implicit operator ActiveID(ActiveData _this)
		{
			return _this.id;
		}
	}
}