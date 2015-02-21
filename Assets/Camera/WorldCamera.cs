using UnityEngine;

namespace Choanji
{
	public class WorldCamera : MonoBehaviour
	{
		public Camera cam;

		void Awake()
		{
			Cameras.world = cam;
		}
	}

}