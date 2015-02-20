using System;
using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji.Battle
{
	public class CardView : MonoBehaviour
	{
		public CardData data { get; private set; }

		public Text name_;
		public Image illust;

		public Action onSelect;
		public Action onShowDetail;

		public bool isSetuped { get { return data != null; } }

		public void Setup(CardData _data)
		{
			if (isSetuped)
			{
				L.E(L.M.CALL_RETRY("setup"));
				return;
			}

			data = _data;
			name_.text = _data.name;
			illust.sprite = R.BattleUI.Spr.CARD_ILLUST_S(_data.key);
		}

		public void OnClick()
		{
			if (Input.GetMouseButtonUp(0))
				onSelect.CheckAndCall();
			else if (Input.GetMouseButtonUp(1))
				onShowDetail.CheckAndCall();
		}
	}
}