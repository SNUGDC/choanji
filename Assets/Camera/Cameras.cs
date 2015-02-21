using Gem;
using UnityEngine;

namespace Choanji
{
	public static class Cameras
	{
		private static Camera mWorld;

		public static Camera world
		{
			get { return mWorld; }
			set
			{
				if (mWorld != null)
					L.E(L.M.CALL_RETRY("set camera"));

				mWorld = value;

				worldMani = null;

				if (world)
				{
					worldMani = world.gameObject.AddIfNotExists<TransformManipulator>();
				}
			}
		}

		public static TransformManipulator worldMani { get; private set; }
	}
}