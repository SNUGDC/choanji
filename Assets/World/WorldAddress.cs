namespace Choanji
{
	using MapKey = WorldBluePrint.Room.Key;

	public struct WorldAddress
	{
		public WorldAddress(string _world, MapKey _map, Coor _coor)
		{
			world = _world;
			map = _map;
			coor = _coor;
		}

		public string world;
		public MapKey map;
		public Coor coor;

		public override string ToString()
		{
			return "( " + world + ", " + map + ", " + coor + " )";
		}
	}
}