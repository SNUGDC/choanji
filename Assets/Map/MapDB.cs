using System;
using System.Collections.Generic;
using Gem;

namespace Choanji
{
	using DB = Dictionary<MapID, MapStatic>;
	
	[Serializable]
	public class MapStatic
	{
		public MapMeta meta;
		[NonSerialized]
		public Grid<TileData> grid;
		[NonSerialized]
		public Prefab prefab;
	}

	public static class MapDB
	{
		private static readonly Path_ BIN_PATH = new Path_("Resources/DB/map.db");

		private static DB sDB;

		public static bool isLoaded { get { return sDB != null; } }

		public static bool TryGet(MapID _id, out MapStatic _data)
		{
			if (!isLoaded) Load();
			return sDB.TryGet(_id, out _data);
		}

		public static MapStatic Get(MapID _id)
		{
			return sDB[_id];
		}

		public static bool Add(MapMeta _meta)
		{
			D.Assert(isLoaded);
			return sDB.TryAdd(_meta, new MapStatic { meta =_meta });
		}

		public static bool Remove(MapID _id)
		{
			D.Assert(isLoaded);
			return sDB.TryRemove(_id);
		}

		public static void Replace(MapMeta _meta)
		{
			Remove(_meta);
			Add(_meta);
		}

		public static void Save()
		{
			if (!isLoaded)
			{
				L.W(L.M.CALL_INVALID);
				return;
			}

			sDB.Enc(BIN_PATH);
		}

		public static bool TryLoad()
		{
			if (isLoaded) 
				return false;

			Load();
			return true;
		}

		public static void Load()
		{
			D.Assert(!isLoaded);
			if (!SerializeHelper.Dec(BIN_PATH, out sDB))
				sDB = new DB();
		}
	}

}