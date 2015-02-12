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
		public TileDoorData(TileDoorKey _key, string _exitWorld, string _exitRoom, string _exitMap, TileDoorKey _exitDoor)
		{
			key = _key;
			exitWorld = _exitWorld;
			exitRoom = WorldBluePrint.Room.MakeKey(_exitRoom);
			exitMap = MapIDHelper.Make(_exitMap);
			exitDoor = _exitDoor;
		}

		public readonly TileDoorKey key;
		public readonly string exitWorld;
		public readonly WorldBluePrint.Room.Key exitRoom;
		public readonly MapID exitMap;
		public readonly TileDoorKey exitDoor;
	}
}