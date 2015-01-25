using System.Collections.Generic;
using System.Linq;
using Gem;
using UnityEngine;

namespace Choanji
{
	public class WorldBluePrint
	{
		public class Room
		{
			public readonly MapID id;
			public readonly PRect rect;

			public Room(MapID _id, PRect _rect)
			{
				id = _id;
				rect = _rect;
			}

			public Vector2 worldPos
			{
				get
				{
					var _pos = (Vector2) rect.org;
					_pos.y += rect.h;
					return _pos;
				}
			}

			public override int GetHashCode()
			{
				return ((int)id) * rect.GetHashCode();
			}
		}

		private readonly List<Room> mRooms = new List<Room>();
		private readonly PRectGroup mRectGroup = new PRectGroup();

		public void Add(Room _room)
		{
			mRooms.Add(_room);
			mRectGroup.Add(_room.rect);
		}

		public Room Contains(Point _rect)
		{
			return mRectGroup.Contains(_rect).Select(_idx => mRooms[_idx]).FirstOrDefault();
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
			var _path = new FullPath(JSON_PATH/(_world + ".json"));

			var _roomsJs = JsonHelper.DataWithRaw(_path);
			if (_roomsJs == null) return null;
			foreach (var _roomJs in _roomsJs.GetListEnum())
			{
				MapStatic _map;
				var _mapID = MapIDHelper.Make((string) _roomJs["map"]);

				if (!MapDB.TryGet(_mapID, out _map))
					continue;

				Point _pos;
				if (!Point.TryParse(_roomJs["pos"], out _pos))
					continue;

				var _rect = new PRect {org = _pos, size = _map.meta.size};
				_ret.Add(new Room(_mapID, _rect));
			}

			return _ret;
		}
	}
}