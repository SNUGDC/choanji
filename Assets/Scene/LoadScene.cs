﻿using Gem;
using UnityEngine;

namespace Choanji
{
	public class LoadScene : MonoBehaviour
	{
		public void TransferToWorld()
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