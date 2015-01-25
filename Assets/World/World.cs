using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace Choanji
{
	using Rooms = Dictionary<WorldBluePrint.Room, GameObject>;

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
				Object.Destroy(_kv.Value);
			mRooms.Clear();
		}

		public void Construct(PRect _rect)
		{
			var _rooms = mBluePrint.Overlaps(_rect);
			foreach (var _room in _rooms)
			{
				if (mRooms.ContainsKey(_room))
					continue;
				var _roomGO = MapDB.Get(_room.id).prefab.Instantiate();
				mRooms.Add(_room, _roomGO);
			}
		}

		public void Destruct(PRect _rect)
		{
			var _removes = new List<WorldBluePrint.Room>();

			foreach (var _kv in mRooms)
			{
				if (_rect.Overlaps(_kv.Key.rect))
					continue;
				Object.Destroy(_kv.Value);
				_removes.Add(_kv.Key);
			}

			foreach (var _room in _removes)
				mRooms.Remove(_room);
		}

	}
}