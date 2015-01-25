using Gem;

namespace Choanji
{
	using TileGrid = Grid<TileData>;
	using StateGrid = Grid<TileState>;

	public class MapDynamic 
	{
		public MapDynamic(MapStatic _static)
		{
			var _grid = _static.grid;
			var _size = _grid.size;

			grid = new StateGrid(_size);
			foreach (var p in _size.Range())
			{
				var _data = _grid[p];
				if (_data == null) continue;
				grid[p] = new TileState(_data);
			}
		}

		public readonly StateGrid grid;
	}
}