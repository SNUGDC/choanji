using Gem;
using UnityEngine;

namespace Choanji
{
	public static class TheCharacter
	{
		private static Character sCh;
		public static Character ch
		{
			get { return sCh; }
			set
			{
				if (ch != null)
				{
					L.W(L.M.SHOULD_NULL("g"));
					Object.Destroy(ch.GetComponent<CharacterInputAgent>());
				}

				sCh = value;
				sCh.transform.SetParent(TheWorld.parent);

				ctrl = sCh.GetComponent<CharacterCtrl>();
				D.Assert(ctrl != null);
				ctrl.onEnterDoor += TeleportNextUpdate;

				D.Assert(ch.GetComponent<CharacterInputAgent>() == null);
				ch.gameObject.AddComponent<CharacterInputAgent>();

				Cameras.worldMani.default_ = DefaultCam;
			}
		}

		public static CharacterCtrl ctrl { get; private set; }

		public static WorldCoor worldCoor
		{
			get { return new WorldCoor(sCh.position); }
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
			if (!ch
			    || (!ch.isMoving && (sLastCamCoor == ch.position)))
			{
				return new TransformManipulator.Result();
			}

			WorldProgress.SetCamDirty();
			sLastCamCoor = ch.position;

			return new TransformManipulator.Result
			{
				pos = ch.transform.position
			};
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

			TheWorld.Read(_address.world);

			var _bluePrint = TheWorld.bluePrint;
			var _room = _bluePrint.Find(_address.map);
			var _coor = new Coor(_room.rect.org) + _address.coor;

			TheWorld.UpdateRect(new PRect(_coor));

			var _hasRoomAndMap = TheWorld.g.Search(_address.map);
			if (_hasRoomAndMap == null)
				return;

			var _localCoor = new LocalCoor(_hasRoomAndMap.Value.Value, _address.coor);
			if (!ctrl.TrySetPosition(_localCoor, true))
				L.E("cannot set position to " + _address);
		}

	}

}