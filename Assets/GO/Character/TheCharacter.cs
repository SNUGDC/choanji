using Gem;
using UnityEngine;

namespace Choanji
{
	public static class TheCharacter
	{
		private static Character sG;
		public static Character g
		{
			get { return sG; }
			set
			{
				if (g != null)
				{
					L.W(L.M.SHOULD_NULL("g"));
					Object.Destroy(g.GetComponent<CharacterInputAgent>());
				}

				sG = value;

				ctrl = sG.GetComponent<CharacterCtrl>();
				D.Assert(ctrl != null);
				ctrl.onEnterDoor += TeleportNextUpdate;

				D.Assert(g.GetComponent<CharacterInputAgent>() == null);
				g.gameObject.AddComponent<CharacterInputAgent>();

				Cameras.gameMani.default_ = DefaultCam;
			}
		}

		public static CharacterCtrl ctrl { get; private set; }

		public static WorldCoor worldCoor
		{
			get { return new WorldCoor(sG.position); }
		}

		public static void Update()
		{
			if (mTeleportAddress.HasValue)
			{
				var _address = mTeleportAddress;
				mTeleportAddress = null;
				TeleportImmediate(_address.Value);
			}
		}

		private static Coor sLastCamCoor;

		private static TransformManipulator.Result DefaultCam(GameObject _go, float _dt)
		{
			if (g && (sLastCamCoor != g.position))
			{
				WorldProgress.SetCamDirty();
				sLastCamCoor = g.position;

				return new TransformManipulator.Result
				{
					pos = g.transform.position
				};
			}

			return new TransformManipulator.Result();
		}

		private static WorldAddress? mTeleportAddress;
		public static void TeleportNextUpdate(WorldAddress _address)
		{
			D.Assert(!mTeleportAddress.HasValue);
			mTeleportAddress = _address;
			WorldProgress.SetCamDirty();
		}

		private static void TeleportNextUpdate(TileDoorData _door)
		{
			MapStatic _exitMap;
			if (!MapDB.TryGet(_door.exitMap, out _exitMap))
				return;

			Coor _coor;
			if (!_exitMap.meta.doors.TryGet(_door.exitDoor, out _coor))
				return;

			TeleportNextUpdate(new WorldAddress(_door.exitWorld, _door.exitRoom, _coor));
		}

		public static void TeleportImmediate(WorldAddress _address)
		{
			D.Assert(!mTeleportAddress.HasValue);

			var _bluePrint = WorldBluePrint.Read(_address.world);
			if (_bluePrint == null) return;

			var _room = _bluePrint.Find(_address.map);
			var _coor = new Coor(_room.rect.org) + _address.coor;

			TheWorld.bluePrint = _bluePrint;
			TheWorld.UpdateRect(new PRect(_coor));

			var _hasRoomAndMap = TheWorld.g.Search(_address.map);
			if (_hasRoomAndMap == null)
				return;

			var _localCoor = new LocalCoor(_hasRoomAndMap.Value.Value, _address.coor);
			if (!ctrl.TrySetPosition(_localCoor))
				L.E("cannot set position to " + _address);
		}

	}

}