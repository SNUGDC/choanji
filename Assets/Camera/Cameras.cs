using Gem;
using UnityEngine;

namespace Choanji
{
	public static class Cameras
	{
		private static Camera mGame;

		public static Camera game
		{
			get { return mGame; }
			set
			{
				if (mGame != null)
					L.E(L.M.CALL_RETRY("set camera"));

				mGame = value;

				gameMani = null;

				if (game)
				{
					gameMani = game.gameObject.AddIfNotExists<TransformManipulator>();
				}
			}
		}

		public static TransformManipulator gameMani { get; private set; }
	}
}