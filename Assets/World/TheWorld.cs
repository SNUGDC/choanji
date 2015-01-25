using Gem;
using UnityEngine;

namespace Choanji
{
	public static class TheWorld
	{
#if UNITY_EDITOR
		private static Vector2? mPosition;
#endif

		private static WorldBluePrint mBluePrint;

		public static WorldBluePrint bluePrint
		{
			get { return mBluePrint; }

			set
			{
				mBluePrint = value;

				if (world != null)
				{
					L.W(L.M.SHOULD_NULL("world"));
					world.Purge();
				}

				world = new World(bluePrint);

#if UNITY_EDITOR
				mPosition = null;
#endif
			}
		}

		public static World world { get; private set; }

		public static void Update()
		{
			if (world == null)
			{
				L.W(L.M.CALL_INVALID);
				return;
			}

			var _cam = Cameras.game;
			D.Assert(_cam);

			var _camPos = _cam.transform.position;

			var _camHSize = _cam.OrthoHSize();

			var _rect = new PRect
			{
				org = Coor.Floor((Vector2) _camPos - _camHSize),
				dst = Coor.Ceiling((Vector2) _camPos + _camHSize),
			};

#if UNITY_EDITOR
			{
				D.Assert(!mPosition.HasValue || mPosition != _rect.c);
				mPosition = _rect.c;
			}
#endif

			world.Construct(_rect);
			world.Destruct(_rect);
		}
	}

}