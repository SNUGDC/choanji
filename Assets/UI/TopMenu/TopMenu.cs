using System;
using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji.UI
{
	public class TopMenu : MonoBehaviour
	{
		public Button deck;
		public Button bag;
		public Button save;
		public Button option;

		public Action onPopupOpened;
		public Action onPopupClosed;

		public void OnDeckClicked()
		{
			var _popup = ThePopup.Open(Popups.PARTY);
			if (!_popup) return;

			_popup.onClose += onPopupClosed;

			var _deck = (DeckPopup)_popup;

			_deck.party = Player.party;
			_deck.afterRefreshPartyTab += () => RefreshPartyTab(_deck);
			RefreshPartyTab(_deck);

			_deck.deckTab.deck = Player.deck;

			onPopupOpened.CheckAndCall();
		}

		private void RefreshPartyTab(DeckPopup _deck)
		{
			_deck.partyTab.stat = Player.totalStat;
		}

		public void OnBagClicked()
		{
			return;
			onPopupOpened.CheckAndCall();
		}

		public void OnSaveClicked()
		{
			if (Disket.Save())
				TheToast.Open("모험을 기록하였다!");
			else
				TheToast.Open("기록할 수 없다.");
		}

		public void OnOptionClicked()
		{
			return;
			onPopupOpened.CheckAndCall();
		}
	}
}