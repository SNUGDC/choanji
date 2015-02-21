using Gem;
using LitJson;

namespace Choanji.ActivePerform
{
	public enum ID
	{
		DMG = 1,
	}

	public class Base
	{
		public Base(string _key)
		{
			id = EnumHelper.ParseOrDefault<ID>(_key);
			key = _key;
		}

		public Base(JsonData _data)
			: this((string)_data["key"])
		{}

		public readonly ID id;
		public readonly string key;
	}

	public sealed class Dmg : Base
	{
		public Dmg(ElementID _ele, HP _dmg)
			: base("DMG")
		{
			dmg = new Damage(_ele, _dmg);
		}

		public Dmg(JsonData _data)
			: base(_data)
		{
			var _ele = ElementDB.Search((string)_data["ele"]);
			var _val = (HP)(int)_data["dmg"];
			dmg = new Damage(_ele, _val);
			accuracy = _data.IntOrDefault("accuracy", 100);
		}

		public readonly Damage dmg;
		public int accuracy = 100;
	}

	public static class Factory
	{
		public static Base Make(JsonData _data)
		{
			ID _id;

			JsonData _keyJs;
			if (!_data.TryGet("key", out _keyJs))
				return null;

			if (!EnumHelper.TryParse((string)_keyJs, out _id))
				return null;

			switch (_id)
			{
				case ID.DMG: return new Dmg(_data);
				default: 
					L.W(L.M.CASE_INVALID(_id));
					return null;
			}
		}
	}

}