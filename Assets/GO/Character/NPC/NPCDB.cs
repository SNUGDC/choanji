using Gem;
using System.Collections.Generic;

namespace Choanji
{
	using Dic = Dictionary<NPCID,NPCData>;

	public static class NPCDB 
	{
		private static readonly Dic mDic = new Dic();

		public static bool isLoaded { get { return !mDic.Empty(); }}

		public static void Load()
		{
			if (isLoaded)
			{
				L.W("trying to load again.");
				return;
			}

			var _data = JsonHelper.DataWithRaw(new FullPath("Resources/DB/npc.json"));
			if (_data == null) return;

			foreach (var _kv in _data.GetDictEnum())
				mDic.Add(NPCHelper.MakeID(_kv.Key), new NPCData(_kv.Value));
		}

		public static NPCData Get(NPCID _id)
		{
			D.Assert(isLoaded);
			return mDic.GetOrDefault(_id);
		}
	}
}