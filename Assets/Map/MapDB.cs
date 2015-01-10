using System.Collections.Generic;
using Gem;
using LitJson;

namespace Choanji
{
	public struct MapData
	{
		public MapData(MapMeta _meta, Grid<JsonData> _grid)
		{
			meta = _meta;
			grid = _grid;
		}

		public readonly MapMeta meta;
		public readonly Grid<JsonData> grid;
	}

	public static class MapDB
	{
		public static void Add(MapData _data)
		{
			sMaps.TryAdd(_data.meta.id, _data);
		}

		public static void Remove(MapID _id)
		{
			sMaps.Remove(_id);
		}

		public static readonly Dictionary<MapID, MapData> sMaps 
			= new Dictionary<MapID, MapData>();
	}

}