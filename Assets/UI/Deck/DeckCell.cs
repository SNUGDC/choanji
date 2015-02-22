using System;
using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji
{
	public class DeckCell : MonoBehaviour
	{
		public Image illust;
		public GameObject passive;
		public new GameObject active;
		public Sprite hidden;

		private Card mCard;
		public CardMode? mMode;

		public bool isHidden { get; private set; }
		public Func<bool> shouldShowPassive;
		public Func<bool> shouldShowActive;

		public Action<Card, CardMode, Action<bool>> equipRequest;
		public Action<Card, Action<bool>> unequipRequest;

		public Action<Card, CardMode?, bool> onPointerHover;

		private void SetModeVisiblity(bool _passive, bool _active)
		{
			passive.SetActive(_passive && shouldShowPassive());
			active.SetActive(_active && shouldShowActive());
		}

		public void Hide()
		{
			if (isHidden)
				return;
			isHidden = true;
			mCard = null;
			illust.sprite = hidden;
			SetModeVisiblity(false, false);
		}

		public void Show(Card _card)
		{
			isHidden = false;
			mCard = _card;
			illust.sprite = R.BattleUI.Spr.CARD_ILLUST_S(_card.data.key);
			SetModeVisiblity(false, false);
		}

		public void OpenSelection()
		{
			if (isHidden)
			{
				L.W("show selection while hidden.");
				return;
			}

			SetModeVisiblity(true, true);
		}

		public void CloseSelection()
		{
			if (mMode.HasValue)
				DoEquip(mMode.Value);
			else 
				SetModeVisiblity(false, false);
		}

		private void DoEquip(CardMode _mode)
		{
			if (isHidden)
			{
				L.W("cannot equip while hidden.");
				return;
			}

			mMode = _mode;

			switch (_mode)
			{
				case CardMode.PASSIVE: 
					SetModeVisiblity(true, false);
					break;
				case CardMode.ACTIVE:
					SetModeVisiblity(false, true);
					break;
			}
		}

		public void Unequip()
		{
			SetModeVisiblity(false, false);
		}

		public void OnPointerUp()
		{
			if (Input.GetMouseButtonUp(0))
			{
				if (!isHidden)
					OpenSelection();
			}
			else if (Input.GetMouseButtonUp(2))
			{
				if (mCard != null)
					unequipRequest(mCard, _confirm => Unequip());
			}
		}

		public void OnPassiveClicked()
		{
			equipRequest(mCard, CardMode.PASSIVE, _confirm => DoEquip(CardMode.PASSIVE));
		}

		public void OnActiveClicked()
		{
			equipRequest(mCard, CardMode.ACTIVE, _confirm => DoEquip(CardMode.ACTIVE));
		}

		public void OnPointerHover(bool _enter)
		{
			if (mCard != null)
				onPointerHover.CheckAndCall(mCard, mMode, _enter);
		}
	}
}