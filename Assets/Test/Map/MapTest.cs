using Choanji;
using Tiled2Unity;
using UnityEngine;

namespace Gem
{
	public class MapTest : MonoBehaviour
	{
		public CharacterCtrl ch;
		public MapDataComp map;

		void Awake()
		{
			MapManager.cur = map;
			ch.curMap = MapManager.cur;
		}
	}
}
