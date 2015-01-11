using System.Collections.Generic;
using Gem;

namespace Choanji
{
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