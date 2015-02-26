using Gem;
using UnityEngine;

namespace Choanji
{
	public class LoadScene : MonoBehaviour
	{
		void Start()
		{
			// note: 미구현 바로 world로 넘어감.
			TransferToWorld();
		}

		public void TransferToWorld()
		{
			Disket.LoadOrDefault("test");
			DisketHelper.SetupCommon();
			Application.LoadLevel("game");
			Timer.g.Add(0.5f, SetupWorld);
		}

		private static void SetupWorld()
		{
			DisketHelper.SetupWorld();
			TheChoanji.g.context = ContextType.WORLD;
		}
	}
}