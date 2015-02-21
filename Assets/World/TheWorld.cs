using Gem;
using LitJson;
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

		public static WorldMeta meta { get; private set; }

		private static WorldBluePrint sBluePrint;

		public static WorldBluePrint bluePrint
		{
			get { return sBluePrint; }

			set
			{
				if (parent == null)
				{
					L.W("no parent. create one.");
					parent = new GameObject().transform;
					parent.name = "World";
				}

				sBluePrint = value;

				if (g != null)
				{
					g.Purge();
					g = null;
				}

				if (sBluePrint == null)
					return;

				g = new World(bluePrint, parent);
				sPosition = NULL;
			}
		}

		private static readonly Path_ JSON_PATH = new Path_("Resources/World");

		public static void Read(string _world)
		{
			var _path = new FullPath(JSON_PATH / (_world + ".json"));
			var _data = JsonHelper.DataWithRaw(_path);
			if (_data == null) return;

			meta = JsonMapper.ToObject<WorldMeta>(_data["meta"].ToReader());
			bluePrint = new WorldBluePrint(_world, _data["bluePrint"]);

			if (string.IsNullOrEmpty(meta.bgm))
			{
				SoundManager.StopMusic();
			}
			else
			{
				var _clip = R.Snd.BGM(meta.bgm);
				if (_clip) 
					SoundManager.Play(_clip, true);
				else 
					SoundManager.StopMusic();
			}
		}

		public static void UpdateCam()
		{
			if (g == null)
				return;

			var _cam = Cameras.world;
			D.Assert(_cam);

			var _camPos = _cam.transform.position;
			var _camHSize = _cam.OrthoHSize();

			var _rect = new PRect
			{
				org = Coor.Floor((Vector2) _camPos - _camHSize),
				dst = Coor.Ceiling((Vector2) _camPos + _camHSize),
			};

			UpdateRect(_rect);

			g.Scissor(new PRect
			{
				org = _rect.org - new Point(10, 10),
				dst = _rect.dst + new Point(10, 10)
			});
		}

		public static void UpdateRect(PRect _rect)
		{
			if (g == null) 
				return;

			if (sPosition == _rect.c)
			{
				L.W(L.M.CALL_RETRY("update"));
				return;
			}

			sPosition = _rect.c;

			g.Construct(_rect);
		}
	}

}