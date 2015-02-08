using System;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji.Battle
{
	[Serializable]
	public class SlotData
	{
		public Sprite icon;
		public Sprite frame;
	}

	public class Slot : MonoBehaviour
	{
		public Image icon;
		public Image frame;

		public void SetData(SlotData _data)
		{
			icon.sprite = _data.icon;
			frame.sprite = _data.frame;
		}
	}
}