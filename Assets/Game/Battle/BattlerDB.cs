using Gem;
using System.Collections.Generic;
using LitJson;

namespace Choanji.Battle
{
	using Dic = Dictionary<BattlerID, BattlerData>;

	public static class BattlerHelper
	{
		public static BattlerID MakeID(string _key)
		{
			return (BattlerID)HashEnsure.Do(_key);
		}
	}

	public class BattlerData
	{
		public BattlerData(string _key, JsonData _data)
		{
			id = BattlerHelper.MakeID(_key);
			key = _key;
			name = (string)_data["name"];
			stat = new StatSet(_data["stat"]);
			party = new Party(_data["party"]);
		}

		public readonly BattlerID id;
		public readonly string key;
		public readonly string name;
		public readonly StatSet stat;
		public readonly Party party;

		public static implicit operator BattlerID(BattlerData _this)
		{
			return _this.id;
		}
	}

	public static class BattlerDB
	{
		private static readonly Dic mDic = new Dic();

		public static bool isLoaded { get { return !mDic.Empty(); } }

		public static void Load()
		{
			if (isLoaded)
			{
				L.W("trying to load again.");
				return;
			}

			var _dataJs = JsonHelper.DataWithRaw(new FullPath("Resources/DB/battler.json"));
			if (_dataJs == null) return;

			foreach (var _kv in _dataJs.GetDictEnum())
			{
				var _data = new BattlerData(_kv.Key, _kv.Value);
				mDic.Add(_data, _data);
			}
		}

		public static BattlerData Get(BattlerID _id)
		{
			D.Assert(isLoaded);
			return mDic.GetOrDefault(_id);
		}
	}

}