using Gem;
using UnityEngine;
using System.Collections;

namespace Choanji
{
	public class LoadScene : MonoBehaviour
	{
		void Start()
		{
			// todo: load scene 기획에 맞추어 제작.
			Invoke("TransferToWorld", 1);
		}

		void TransferToWorld()
		{
			Disket.LoadOrDefault("test");
			DisketHelper.SetupCommon();
			Application.LoadLevel("world");
			Timer.g.Add(0.5f, SetupWorld);
		}

		private static void SetupWorld()
		{
			if (TheChoanji.g.context != ContextType.WORLD)
				return;
			DisketHelper.SetupWorld();
		}
	}
}