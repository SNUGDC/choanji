using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace Choanji
{
	using Rooms = Dictionary<WorldBluePrint.Room, Map>;

	public class World
	{
		private readonly WorldBluePrint mBluePrint;

		private readonly Rooms mRooms = new Rooms();

		public World(WorldBluePrint _bluePrint)
		{
			mBluePrint = _bluePrint;
		}

		public void Purge()
		{
			foreach (var _kv in mRooms)
				Object.Destroy(_kv.Value.go);
			mRooms.Clear();
		}

		public void Construct(PRect _rect)
		{
			var _rooms = mBluePrint.Overlaps(_rect);
			foreach (var _room in _rooms)
			{
				if (mRooms.ContainsKey(_room))
					continue;
				var _static = MapDB.Get(_room.id);
				var _roomGO = _static.prefab.Instantiate();
				_roomGO.transform.position = _room.worldPos;
				mRooms.Add(_room, new Map(_static, _roomGO));
			}
		}

		public void Scissor(PRect _rect)
		{
			var _removes = new List<WorldBluePrint.Room>();

			foreach (var _kv in mRooms)
			{
				if (_rect.Overlaps(_kv.Key.rect))
					continue;
				Object.Destroy(_kv.Value.go);
				_removes.Add(_kv.Key);
			}

			foreach (var _room in _removes)
				mRooms.Remove(_room);
		}

		public Map Contains(WorldCoor p)
		{
			var _room = mBluePrint.Contains(p.val);
			if (_room == null) 
				return null;

			Map _ret;
			mRooms.TryGet(_room, out _ret);
			return _ret;
		}

		public LocalCoor? SearchMapAndTile(WorldCoor _pos)
		{
			var _map = Contains(_pos);
			if (_map != null)
				return _map.Convert(_pos);
			else
				return null;
		}
	}
}