#if UNITY_EDITOR
using Gem;
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
			}
		}
	}
}

#endif