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
				D.Assert(g.GetComponent<CharacterInputAgent>() == null);
				g.gameObject.AddComponent<CharacterInputAgent>();

				Cameras.gameMani.default_ = DefaultCam;
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

	}

}