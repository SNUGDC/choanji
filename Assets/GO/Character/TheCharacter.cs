using Gem;
using UnityEngine;

namespace Choanji
{
	public static class TheCharacter
	{
		private static Character sG;
		private static CharacterCtrl sCtrl;

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
				sCtrl = sG.GetComponent<CharacterCtrl>();
				D.Assert(sCtrl != null);

				D.Assert(g.GetComponent<CharacterInputAgent>() == null);
				g.gameObject.AddComponent<CharacterInputAgent>();

				Cameras.gameMani.default_ = DefaultCam;
			}
		}

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

		public static void LateUpdate()
		{
			camDirty = false;
		}

		public static bool camDirty { get; private set; }
		private static Coor sLastCamCoor;

		private static TransformManipulator.Result DefaultCam(GameObject _go, float _dt)
		{
			if (g && (sLastCamCoor != g.position))
			{
				camDirty = true;
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
			camDirty = true;
		}

		public static void TeleportImmediate(WorldAddress _address)
		{
			D.Assert(!mTeleportAddress.HasValue);

			var _bluePrint = WorldBluePrint.Read(_address.world);
			if (_bluePrint == null) return;
			TheWorld.bluePrint = _bluePrint;
			TheWorld.Update();

			var _hasRoomAndMap = TheWorld.g.Search(_address.map);
			if (_hasRoomAndMap == null)
				return;

			var _map = _hasRoomAndMap.Value.Value;
			
			if (!sCtrl.TrySetPosition(new LocalCoor(_map, _address.coor)))
				L.E("cannot set position to " + _address);
		}

	}

}