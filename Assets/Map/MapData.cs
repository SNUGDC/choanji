using System;
using Gem;

namespace Choanji
{
	[Serializable]
	public class MapData 
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

				if (grid != null)
				{
					L.E(L.M.CALL_RETRY("set grid"));
					return;
				}

				mGrid = value;
			}
		}

		[NonSerialized]
		private Grid<TileState> mStates;
		public Grid<TileState> states
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

				mStates = new Grid<TileState>(grid.size);

				foreach (var p in grid.size.Range())
				{
					var _data = grid[p];
					if (_data == null) continue;
					mStates[p] = new TileState(_data);
				}

				return mStates;
			}
		}

		private static string BinPath(string _name)
		{
			return "Assets/Tiled2Unity/Imported/" + _name + ".mapData.bin";
		}

		public static MapData Load(string _name)
		{
			return SerializeHelper.DeserializeFile<MapData>(BinPath(_name));
		}

#if UNITY_EDITOR
		public void Save(string _name)
		{
			this.SerializeToFile(BinPath(_name));
		}
#endif

	}
}