using System.Collections.Generic;
using Gem;

namespace Choanji
{
	using DB = List<ElementData>;
	using Dic = Dictionary<string, ElementData>;
	using Stash = List<ElementRaw>;
	using StashMap = Dictionary<string, ElementID>;

	public static class ElementDB
	{
		private static ElementID sAllocID;

		private static readonly DB sDB = new DB(ElementConst.MAX);
		private static readonly Dic sDic = new Dic();

		private static readonly Stash sStash = new Stash(ElementConst.MAX);
		private static readonly StashMap sStashMap = new StashMap(ElementConst.MAX);

		static ElementDB()
		{
			Load();
		}

		public static bool awake { get { return true; } }
		public static bool isLoaded { get { return sDB.Count > 0; }}

		private static ElementID AllocID(string _key)
		{
			D.Assert(!sDic.ContainsKey(_key));
			return ++sAllocID;
		}

		private static void Add(ElementRaw _data)
		{
			D.Assert(!isLoaded);
			if (sStashMap.TryAdd(_data.key, AllocID(_data.key)))
				sStash.Add(_data);
		}

		public static void Commit()
		{
 			D.Assert(!isLoaded);
			sDB.Capacity = sStash.Count;

			foreach (var _raw in sStash)
			{
				var _data = new ElementData(_raw, sStashMap);
				sDB.Add(_data);
				sDic.Add(_data.key, _data);
			}

			sStashMap.Clear();
			sStash.Clear();
		}

		public static IEnumerable<ElementData> GetEnum()
		{
			D.Assert(isLoaded);
			return sDB;
		}

		public static ElementData Get(ElementID _key)
		{
			D.Assert(isLoaded);
			return sDB[(int)_key - 1];
		}

		public static ElementData Search(string _key)
		{
			D.Assert(isLoaded);
			ElementData _data;
			sDic.TryGet(_key, out _data);
			return _data;
		}

		private static void Load()
		{
			if (isLoaded)
			{
				L.W(L.M.CALL_RETRY("load"));
				return;
			}

			var _csv = CSVHelper.Open(new FullPath("Resources/DB/element.csv"));
			while (!_csv.Eof)
			{
				Add(new ElementRaw(_csv.Row()));
				_csv.Next();
			}

			Commit();
		}
	}
}