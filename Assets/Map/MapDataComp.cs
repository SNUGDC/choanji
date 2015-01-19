using Gem;
using UnityEngine;

namespace Choanji
{
	public class MapDataComp : MonoBehaviour
	{
		[HideInInspector]
		public string binName;

		public bool isLoaded { get { return data != null; } }
		public MapMetaAndGrid data { get; private set; }

		void Awake()
		{
			Load();
		}

		private void Load()
		{
			if (isLoaded)
			{
				L.W(L.M.CALL_RETRY("Load"));
				return;
			}

			MapMetaAndGrid _data;

			if (MapDB.TryGet(MapIDHelper.Make(binName), out _data))
			{
				if (_data.grid == null) 
					_data.grid = MapUtil.LoadTileGrid(binName);
				data = _data;
			}
		}
	}
}