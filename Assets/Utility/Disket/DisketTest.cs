#if UNITY_EDITOR
using UnityEngine;

namespace Choanji
{
	public class DisketTest : MonoBehaviour
	{

		void Start()
		{
			if (!Disket.isLoaded)
			{
				Disket.LoadOrDefault("test");
				DisketHelper.SetupCommon();
				DisketHelper.SetupWorld();
				TheChoanji.g.context = ContextType.WORLD;
			}
		}
	}
}

#endif