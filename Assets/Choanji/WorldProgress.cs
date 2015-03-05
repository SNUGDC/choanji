namespace Choanji
{
	public static class WorldProgress
	{
		private static bool sCamDirty = true;
		public static void SetCamDirty() { sCamDirty = true; }

		public static void Update()
		{
			TheCharacter.Update();

			if (sCamDirty)
			{
				TheWorld.UpdateCam();
				sCamDirty = false;
			}
		}

		public static void LateUpdate()
		{
		}
	}

}