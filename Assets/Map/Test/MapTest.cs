using Choanji;
using UnityEngine;

namespace Gem
{
	public class MapTest : MonoBehaviour
	{
		public CharacterCtrl[] chs;
		public MapDataComp map;

		void Awake()
		{
			MapManager.cur = map;
			foreach (var _ch in chs)
				_ch.curMap = map;
		}
	}
}
