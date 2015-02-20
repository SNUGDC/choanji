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

			var _data = JsonHelper.DataWithRaw(new FullPath("Resources/DB/card.json"));
			if (_data == null) return;

			foreach (var _kv in _data.GetDictEnum())
			{
				var _id = CardHelper.MakeID(_kv.Key);
				mDic.Add(_id, new CardData(_kv.Key, _kv.Value));
			}
		}

		public static CardData Get(CardID _id)
		{
			D.Assert(isLoaded);
			return mDic.GetOrDefault(_id);
		}
	}
}