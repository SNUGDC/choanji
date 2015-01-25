using UnityEngine;

namespace Choanji
{
	public class GameCamera : MonoBehaviour
	{
		public Camera cam;

		void Awake()
		{
			Cameras.game = cam;
		}
	}

}