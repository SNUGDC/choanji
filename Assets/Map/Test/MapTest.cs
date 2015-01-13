using Choanji;
using UnityEngine;

namespace Gem
{
	public class MapTest : MonoBehaviour
	{
		public CharacterCtrl[] ch;
		public MapDataComp map;

		void Awake()
		{
			MapManager.cur = map;
			foreach (var _ch in ch)
				_ch.curMap = map;
		}
	}
}
