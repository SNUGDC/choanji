using UnityEngine;

namespace Choanji
{

	public class DialogInit : MonoBehaviour
	{
		public Dialog dialogPrefab;

		void Start()
		{
			TheDialog.g.dialogPrefab = dialogPrefab;
		}
	}

}
