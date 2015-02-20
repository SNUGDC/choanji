using Gem;
using System.Collections.Generic;

namespace Choanji
{
	using PassiveDic = Dictionary<PassiveID, PassiveData>;
	using ActiveDic = Dictionary<ActiveID, ActiveData>;

	public static class MoveDB
	{
		private static readonly PassiveDic mPassiveDic = new PassiveDic();
		private static readonly ActiveDic mActiveDic = new ActiveDic();

		public static bool isLoaded { get { return !mPassiveDic.Empty(); } }

		public static void Load()
		{
			if (isLoaded)
			{
				L.W("trying to load again.");
				return;
			}

			{
				var _dataJs = JsonHelper.DataWithRaw(new FullPath("Resources/DB/passive.json"));
				if (_dataJs == null) return;

				foreach (var _kv in _dataJs.GetDictEnum())
				{
					var _data = new PassiveData(_kv.Key, _kv.Value);
					mPassiveDic.Add(_data, _data);
				}
			}

			{
				var _dataJs = JsonHelper.DataWithRaw(new FullPath("Resources/DB/active.json"));
				if (_dataJs == null) return;

				foreach (var _kv in _dataJs.GetDictEnum())
				{
					var _data = new ActiveData(_kv.Key, _kv.Value);
					mActiveDic.Add(_data, _data);
				}
			}
		}

		public static PassiveData Get(PassiveID _id)
		{
			D.Assert(isLoaded);
			return mPassiveDic.GetOrDefault(_id);
		}

		public static ActiveData Get(ActiveID _id)
		{
			D.Assert(isLoaded);
			return mActiveDic.GetOrDefault(_id);
		}
	}
}