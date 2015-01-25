using System;
using Gem;
using UnityEngine;

namespace Choanji
{
	[Serializable]
	public class MapStatic
	{
		public MapStatic(MapMeta _meta)
		{
			meta = _meta;
		}

		public readonly MapMeta meta;

		[NonSerialized] 
		private Grid<TileData> mGrid;
		public Grid<TileData> grid
		{
			get { return mGrid ?? (mGrid = MapUtil.LoadTileGrid(meta.name)); }

			set
			{
				if (mGrid != null)
					L.W(L.M.SHOULD_NULL("grid"));
				mGrid = value;
			}
		}

		[NonSerialized]
		private Prefab mPrefab;

		public Prefab prefab
		{
			get
			{
				if (mPrefab.go == null)
				{
					var _prefab = Resources.Load<GameObject>(PrefabPath());
					mPrefab = new Prefab(_prefab);
				}

				return mPrefab;
			}
		}

		private string PrefabPath()
		{
			return meta.name;
		}
	}
}