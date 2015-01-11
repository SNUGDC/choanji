using Choanji;
using UnityEngine;

namespace Gem
{
	public class MapTest : MonoBehaviour
	{
		public CharacterCtrl ch;

		void Awake()
		{
			MapManager.cur = MakeData();
			ch.curMap = MapManager.cur;
		}

		static MapData MakeData()
		{
			var _meta = new MapMeta("test");
			var _grid = new Grid<TileData>(new Point(32, 48));
			_grid[new Point(14, 41)] = new TileData();
			_grid[new Point(15, 41)] = new TileData();

			var _data = new MapData(_meta, _grid);
			return _data;
		}

	}
}
