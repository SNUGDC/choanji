using Gem;

namespace Choanji
{
	public static class MapUtil
	{
		private static Path_ TileGridPath(string _name)
		{
			return new Path_("Resources/Map/" + _name + ".bin");
		}

		public static Grid<TileData> LoadTileGrid(string _name)
		{
			Grid<TileData> _ret;
			SerializeHelper.Dec(TileGridPath(_name), out _ret);
			return _ret;
		}

#if UNITY_EDITOR
		public static void SaveTileGrid(string _name, Grid<TileData> _data)
		{
			_data.Enc(TileGridPath(_name));
		}
#endif
	}

}