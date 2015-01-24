using System.Collections.Generic;
using System.Linq;
using Gem;

namespace Choanji
{
	class WorldBluePrint
	{
		public class Room
		{
			public MapMeta meta;
			public Point pos;
		}

		private readonly List<Room> mRooms = new List<Room>();
		private readonly PRectGroup mRectGroup = new PRectGroup();

		public void Add(Room _room)
		{
			D.Assert(_room.meta != null);
			mRooms.Add(_room);
			mRectGroup.Add(new PRect { org = _room.pos, size = _room.meta.size });
		}

		public List<Room> Overlaps(PRect _rect)
		{
			return mRectGroup.Overlaps(_rect).Select(_idx => mRooms[_idx]).ToList();
		}

		private static readonly Path_ JSON_PATH = new Path_("Resources/World");

		// todo: Read binary when release.
		public static WorldBluePrint Read(string _world)
		{
			var _ret = new WorldBluePrint();

			var _roomsJs = JsonHelper.DataWithRaw(JSON_PATH / (_world + ".json"));
			if (_roomsJs == null) return null;
			foreach (var _roomJs in _roomsJs.GetListEnum())
			{
				MapMetaAndGrid _map;
				var _mapID = MapIDHelper.Make((string) _roomJs["map"]);

				if (!MapDB.TryGet(_mapID, out _map))
					continue;

				Point _pos;
				if (!Point.TryParse(_roomJs["pos"], out _pos))
					continue;
				
				_ret.Add(new Room { meta = _map.meta, pos = _pos });
			}

			return _ret;
		}
	}
}