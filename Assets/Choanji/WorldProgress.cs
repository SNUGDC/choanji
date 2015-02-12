namespace Choanji
{
	public static class WorldProgress
	{
		public static bool sCamDirty = true;
		public static void SetCamDirty() { sCamDirty = true; }

		public static void Update()
		{
			TheCharacter.Update();

			if (sCamDirty)
			{
				TheWorld.Update();
				sCamDirty = false;
			}
		}

		public static void LateUpdate()
		{
		}
	}

}