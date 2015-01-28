﻿namespace Choanji
{
	public static class WorldProgress
	{
		public static void Update()
		{
			if (TheCharacter.camDirty)
				TheWorld.Update();
		}

		public static void LateUpdate()
		{
			TheCharacter.LateUpdate();
		}
	}

}