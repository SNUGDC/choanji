using Gem;

namespace Choanji
{
	public class MapData
	{
		public MapData(MapMeta _meta, Grid<TileData> _grid)
		{
			meta = _meta;
			grid = _grid;
			states = new Grid<TileState>(grid.size);

			for (var _x = 0; _x != grid.w; ++_x)
			{
				for (var _y = 0; _y != grid.h; ++_y)
				{
					var _data = grid[new Point(_x, _y)];
					if (_data == null) continue;
					states[new Point(_x, _y)] = new TileState(_data);
				}
			}
		}

		public readonly MapMeta meta;
		public readonly Grid<TileData> grid;
		public readonly Grid<TileState> states;
	}
}