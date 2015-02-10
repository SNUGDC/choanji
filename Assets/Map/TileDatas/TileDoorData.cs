using System;
using Gem;

namespace Choanji
{
	[Serializable]
	public enum TileDoorKey {}

	public static class TileDataHelper
	{
		public static TileDoorKey MakeDoorKey(string _key)
		{
			return (TileDoorKey)HashEnsure.Do(_key);
		}
	}

	[Serializable]
	public class TileDoorData
	{
		public TileDoorData(TileDoorKey _key, string _exitWorld, string _exitMap, TileDoorKey _exitDoor)
		{
			key = _key;
			exitWorld = _exitWorld;
			exitMap = _exitMap;
			exitDoor = _exitDoor;
		}

		public TileDoorKey key;
		public string exitWorld;
		public string exitMap;
		public TileDoorKey exitDoor;
	}
}