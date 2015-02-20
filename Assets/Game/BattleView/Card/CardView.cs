using System;
using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji.Battle
{
	public class CardView : MonoBehaviour
	{
		public Card card { get; private set; }

		public Text name_;
		public Image illust;

		public Action onSelect;
		public Action onCancel;
		public Action onShowDetail;
		public Action<bool> onPointerOver;

		public bool isSetuped { get { return card != null; } }

		public void Setup(Card _card)
		{
			if (isSetuped)
			{
				L.E(L.M.CALL_RETRY("setup"));
				return;
			}

			card = _card;
			name_.text = _card.data.name;
			illust.sprite = R.BattleUI.Spr.CARD_ILLUST_S(_card.data.key);
		}

		public void OnClick()
		{
			if (Input.GetMouseButtonUp(0))
				onSelect.CheckAndCall();
			else if (Input.GetMouseButtonUp(2))
				onCancel.CheckAndCall();
			else if (Input.GetMouseButtonUp(1))
				onShowDetail.CheckAndCall();
		}

		public void OnPointerOver(bool _enter)
		{
			onPointerOver.CheckAndCall(_enter);
		}
	}
}