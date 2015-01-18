using UnityEngine;

namespace Choanji
{

	public class DialogInit : MonoBehaviour
	{
		public Dialog prefab;
		public RectTransform parent;

		void Start()
		{
			TheDialog.g.dialogPrefab = prefab;
			TheDialog.g.dialogParent = parent;
		}
	}

}
