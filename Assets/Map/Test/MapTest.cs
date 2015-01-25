using Choanji;
using UnityEngine;

namespace Gem
{
	public class MapTest : MonoBehaviour
	{
		public CharacterCtrl[] chs;
		public MapStaticComp map;

		void Awake()
		{
			MapManager.cur = new MapData(map.data);

			foreach (var _ch in chs)
				_ch.curMap = MapManager.cur;
		}
	}
}
