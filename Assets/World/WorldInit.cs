using UnityEngine;

namespace Choanji
{
	public class WorldInit : MonoBehaviour
	{
		public Transform parent;

		void Start () {
			TheWorld.parent = parent;
			TheChoanji.g.context = ContextType.WORLD;
		}
	}
}