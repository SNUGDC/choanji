using Gem;
using UnityEngine;

namespace Choanji
{
	public class MapData : MonoBehaviour
	{
		public MapMeta meta;

		private Grid<TileData> mGrid;
		public Grid<TileData> grid
		{
			get { return mGrid; }

			set
			{
				if (value == null) 
					return;

				if ((grid != null) || (states != null))
				{
					L.E(L.M.CALL_RETRY("set grid"));
					return;
				}

				mGrid = value;

				states = new Grid<TileState>(grid.size);

				foreach (var p in grid.size.Range())
				{
					var _data = grid[p];
					if (_data == null) continue;
					states[p] = new TileState(_data);
				}
			}
		}
		public Grid<TileState> states { get; private set; }
	}
}