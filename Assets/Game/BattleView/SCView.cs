using UnityEngine;
using UnityEngine.UI;

namespace Choanji.Battle
{
	public class SCView : MonoBehaviour
	{
		public Image icon;

		public SC sc
		{
			set
			{
				icon.sprite = SCDB.g[value].uiIcon;
			}
		}
	}

}