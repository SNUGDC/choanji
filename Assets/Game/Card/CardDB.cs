using Gem;
using System.Collections.Generic;

namespace Choanji
{
	using Dic = Dictionary<CardID, CardData>;

	public static class CardDB
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

			var _dataJs = JsonHelper.DataWithRaw(new FullPath("Resources/DB/card.json"));
			if (_dataJs == null) return;

			foreach (var _kv in _dataJs.GetDictEnum())
			{
				var _data = new CardData(_kv.Key, _kv.Value);
				mDic.Add(_data, _data);
			}
		}

		public static CardData Get(CardID _id)
		{
			if (!isLoaded) Load();
			return mDic.GetOrDefault(_id);
		}
	}
}