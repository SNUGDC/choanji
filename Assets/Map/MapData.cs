using System;
using Gem;

namespace Choanji
{
	using TileGrid = Grid<TileData>;
	using StateGrid = Grid<TileState>;

	public class MapData 
	{
		public MapData() { }

		public MapData(MapMetaAndGrid _data)
		{
			meta = _data.meta;
			grid = _data.grid;
		}

		public MapMeta meta;

		private TileGrid mGrid;
		public TileGrid grid
		{
			get { return mGrid; }

			set
			{
				if (value == null) 
					return;

				if (grid != null)
				{
					L.E(L.M.CALL_RETRY("set grid"));
					return;
				}

				mGrid = value;
			}
		}

		private StateGrid mStates;
		public StateGrid states
		{
			get
			{
				if (grid == null)
				{
					L.E(L.M.SHOULD_NOT_NULL("grid"));
					return mStates;
				}

				if (mStates != null)
					return mStates;

				mStates = new StateGrid(grid.size);

				foreach (var p in grid.size.Range())
				{
					var _data = grid[p];
					if (_data == null) continue;
					mStates[p] = new TileState(_data);
				}

				return mStates;
			}
		}

	}
}