using System;
using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji.Battle
{
	public class SelectionCell : MonoBehaviour
	{
		public Image renderer_;
		public Action onCancel;

		public string card
		{
			set { renderer_.sprite = R.BattleUI.Spr.CARD_ILLUST_S(value); }
		}

		public void OnClick()
		{
			if (Input.GetMouseButtonUp(2))
				onCancel.CheckAndCall();
		}
	}
}