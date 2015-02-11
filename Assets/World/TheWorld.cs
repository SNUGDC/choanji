using Gem;
using UnityEngine;

namespace Choanji
{
	public static class TheWorld
	{
		static TheWorld()
		{}

		public static World g { get; private set; }

		private static Transform sParent;
		public static Transform parent
		{
			get { return sParent; }
			set
			{
				if (sBluePrint != null)
					L.W("trying to change parent after set blueprint.");
				sParent = value;
			}
		}

		private static readonly Vector2 NULL = new Vector2(-34873453, -34537804);
		private static Vector2 sPosition = NULL;

		private static WorldBluePrint sBluePrint;

		public static WorldBluePrint bluePrint
		{
			get { return sBluePrint; }

			set
			{
				sBluePrint = value;

				if (g != null)
				{
					g.Purge();
					g = null;
				}

				if (value == null)
					return;

				D.Assert(parent != null);

				g = new World(bluePrint, parent);
				sPosition = NULL;
			}
		}

		public static void Update()
		{
			var _cam = Cameras.game;
			D.Assert(_cam);

			var _camPos = _cam.transform.position;
			var _camHSize = _cam.OrthoHSize();

			var _rect = new PRect
			{
				org = Coor.Floor((Vector2) _camPos - _camHSize),
				dst = Coor.Ceiling((Vector2) _camPos + _camHSize),
			};

			if (sPosition == _rect.c)
			{
				L.W(L.M.CALL_RETRY("update"));
				return;
			}

			sPosition = _rect.c;

			g.Construct(_rect);
			g.Scissor(_rect);
		}
	}

}