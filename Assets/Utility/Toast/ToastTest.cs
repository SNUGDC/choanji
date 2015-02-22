#if UNITY_EDITOR
using UnityEngine;

namespace Choanji
{
	public class ToastTest : MonoBehaviour
	{
		private void Start()
		{
			TheToast.Open("toast");
		}
	}
}

#endif