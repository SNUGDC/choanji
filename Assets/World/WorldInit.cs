using UnityEngine;

namespace Choanji
{
	public class WorldInit : MonoBehaviour
	{
		void Start () {
			TheChoanji.g.context = ContextType.WORLD;
		}
	}
}