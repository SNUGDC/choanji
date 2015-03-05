using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gem;
using LitJson;
using UnityEngine;

namespace Choanji
{
	public class WorldBluePrint
	{
		[DebuggerDisplay("key={key},map={map},rect={rect}")]
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

		public WorldBluePrint(string _name, JsonData _data)
		{
			name = _name;

			foreach (var _room in _data.GetListEnum())
			{
				MapStatic _map;

				var _mapID = MapIDHelper.Make((string)_room["map"]);
				if (!MapDB.TryGet(_mapID, out _map))
					continue;

				Point _pos;
				if (!Point.TryParse(_room["pos"], out _pos))
					continue;

				var _rect = new PRect { org = _pos, size = _map.meta.size };

				JsonData _key;
				Add(_room.TryGet("key", out _key)
					? new Room((string)_key, _mapID, _rect)
					: new Room(_mapID, _rect));
			}
		}

		public void Add(Room _room)
		{
			mRooms.Add(_room);
#if UNITY_EDITOR
			var _overlaps = mRectGroup.Overlaps(_room.rect);
			if (_overlaps != null && !_overlaps.Empty())
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
			var _contain = mRectGroup.Contains(_rect);
			if (_contain == null) return null;
			return mRooms[_contain.First()];
		}

		public List<Room> Overlaps(PRect _rect)
		{
			var _overlaps = mRectGroup.Overlaps(_rect);
			if (_overlaps == null || _overlaps.Empty()) return null;
			return _overlaps.Select(_idx => mRooms[_idx]).ToList();
		}
	}
}