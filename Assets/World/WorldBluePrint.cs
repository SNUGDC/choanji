using System.Collections.Generic;
using System.Linq;
using Gem;
using LitJson;
using UnityEngine;

namespace Choanji
{
	public class WorldBluePrint
	{
		public class Room
		{
			public enum Key {}

			public readonly Key key;
			public readonly MapID map;
			public readonly PRect rect;
			
			public Room(MapID _map, PRect _rect)
			{
				map = _map;
				rect = _rect;
			}

			public Room(string _key, MapID _map, PRect _rect)
				: this(_map, _rect)
			{
				key = MakeKey(_key);
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
				return ((int)map) * rect.GetHashCode();
			}

			public static Key MakeKey(string _key)
			{
				return (Key)HashEnsure.Do(_key);
			}

			public static implicit operator Key(Room _this)
			{
				return _this.key;
			}
		}

		public readonly string name;
		private readonly List<Room> mRooms = new List<Room>();
		private readonly PRectGroup mRectGroup = new PRectGroup();
		private readonly Dictionary<Room.Key, Room> mDic = new Dictionary<Room.Key, Room>();

		public WorldBluePrint(string _name)
		{
			name = _name;
		}

		public void Add(Room _room)
		{
			mRooms.Add(_room);
#if UNITY_EDITOR
			if (!mRectGroup.Overlaps(_room.rect).Empty())
				L.E("overlap detected.");
#endif
			mRectGroup.Add(_room.rect);

			if (_room != default(Room.Key))
				mDic.Add(_room, _room);
		}

		public Room Find(Room.Key _key)
		{
			Room _ret;
			mDic.TryGet(_key, out _ret);
			return _ret;
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
			var _ret = new WorldBluePrint(_world);
			var _path = new FullPath(JSON_PATH/(_world + ".json"));

			var _roomsJs = JsonHelper.DataWithRaw(_path);
			if (_roomsJs == null) return null;
			foreach (var _roomJs in _roomsJs.GetListEnum())
			{
				MapStatic _map;

				var _mapID = MapIDHelper.Make((string)_roomJs["map"]);
				if (!MapDB.TryGet(_mapID, out _map))
					continue;

				Point _pos;
				if (!Point.TryParse(_roomJs["pos"], out _pos))
					continue;

				var _rect = new PRect {org = _pos, size = _map.meta.size};

				JsonData _key;
				_ret.Add(_roomJs.TryGet("key", out _key) 
					? new Room((string) _key, _mapID, _rect) 
					: new Room(_mapID, _rect));
			}

			return _ret;
		}
	}
}