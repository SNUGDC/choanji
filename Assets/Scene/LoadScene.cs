using Gem;
using UnityEngine;

namespace Choanji
{
	public class LoadScene : MonoBehaviour
	{
		public void TransferToWorld()
		{
			Disket.LoadOrDefault("test");
			DisketHelper.SetupCommon();
			Application.LoadLevel("game");
			Timer.g.Add(0, SetupWorld);
		}

		private static void SetupWorld()
		{
			DisketHelper.SetupWorld();
			TheChoanji.g.context = ContextType.WORLD;
		}
	}
}