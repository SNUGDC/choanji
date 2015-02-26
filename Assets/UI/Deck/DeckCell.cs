using System;
using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji
{
	public class DeckCell : MonoBehaviour
	{
		public static DeckCell sCur;
		public DeckCell cur
		{
			get { return sCur; }
			set
			{
				sCur = value;
				if (cur != null)
					lastFocus = cur;
			}
		}

		public static DeckCell lastFocus;

		public Image back;
		public Image hidden;

		public Image illust;
		public GameObject passive;
		public new GameObject active;

		public Card card { get; private set; }
		public CardMode? mode { get; private set; }

		public bool isHidden { get; private set; }
		public bool isSelectionOpened { get { return this == cur; } }
		public Func<bool> canSelectPassive;
		public Func<bool> canSelectActive;

		public Action<Card, CardMode, Action<bool>> equipRequest;
		public Action<Card, Action<bool>> unequipRequest;

		public Action<Card, CardMode?, bool> onPointerHover;

		private void SetModeVisiblity(bool _passive, bool _active)
		{
			passive.SetActive(_passive);
			active.SetActive(_active);
		}

		public void Hide()
		{
			if (isHidden)
				return;
			if (cur == this)
				cur = null;
			isHidden = true;
			hidden.gameObject.SetActive(true);
			back.color = Color.gray;
			card = null;
			illust.gameObject.SetActive(false);
			illust.sprite = null;
			SetModeVisiblity(false, false);
		}

		public void Show(Card _card)
		{
			isHidden = false;
			hidden.gameObject.SetActive(false);
			card = _card;
			back.color = UnityHelper.ColorOrDefault(card.data.theme, CardData.DEFAULT_THEME);
			illust.gameObject.SetActive(true);
			illust.sprite = R.Spr.CARD_ILLUST_S(_card.data.key);
			SetModeVisiblity(false, false);
		}

		public void OpenSelection()
		{
			if (isHidden)
			{
				L.W("show selection while hidden.");
				return;
			}

			if (cur)
				cur.CloseSelection();

			cur = this;

			SetModeVisiblity(true, true);
		}

		public void CloseSelection()
		{
			if (cur == this)
				cur = null;

			if (mode.HasValue)
				DoEquip(mode.Value);
			else 
				SetModeVisiblity(false, false);
		}

		public void Equip(CardMode _mode)
		{
			DoEquip(_mode);
		}

		private void DoEquip(CardMode _mode)
		{
			if (cur == this)
				cur = null;

			if (isHidden)
			{
				L.W("cannot equip while hidden.");
				return;
			}

			mode = _mode;

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
			if (isHidden)
				return;

			if (Input.GetMouseButtonUp(0))
			{
				OpenSelection();
			}
			else if (Input.GetMouseButtonUp(2))
			{
				if (isSelectionOpened)
					CloseSelection();
				else if (card != null)
					unequipRequest(card, _confirm =>
					{
						if (_confirm) Unequip();
					});
			}
		}

		public void OnPassiveClicked()
		{
			if (canSelectPassive())
				equipRequest(card, CardMode.PASSIVE, _confirm => DoEquip(CardMode.PASSIVE));
			else
				TheToast.Open("파티가 가득 찼다!");
		}

		public void OnActiveClicked()
		{
			if (canSelectActive())
				equipRequest(card, CardMode.ACTIVE, _confirm => DoEquip(CardMode.ACTIVE));
			else
				TheToast.Open("파티가 가득 찼다!");
		}

		public void OnPointerHover(bool _enter)
		{
			if (card != null)
				onPointerHover.CheckAndCall(card, mode, _enter);
		}
	}
}