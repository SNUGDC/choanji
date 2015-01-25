using Gem;
using UnityEngine;

namespace Choanji
{
	public static class TheWorld
	{
		static TheWorld()
		{
			// note: avoid null pointer exception.
			g = new World(new WorldBluePrint());
		}

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

				if (g != null)
				{
					L.W(L.M.SHOULD_NULL("world"));
					g.Purge();
				}

				g = new World(bluePrint);

#if UNITY_EDITOR
				mPosition = null;
#endif
			}
		}

		public static World g { get; private set; }

		public static void Update()
		{
			if (g == null)
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

			g.Construct(_rect);
			g.Destruct(_rect);
		}
	}

}