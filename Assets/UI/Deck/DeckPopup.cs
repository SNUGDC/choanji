using System;
using Gem;

namespace Choanji.UI
{
	public class DeckPopup : Popup
	{
		public PartyTab partyTab;
		public DeckTab deckTab;

		public Action afterRefreshPartyTab;

		public Party party
		{
			set
			{
				partyTab.party = value;
				deckTab.party = value;
			}
		}

		void Start()
		{
			Timer.g.Add(0, () => deckTab.gameObject.SetActive(false));
		}

		public void OnPartyTabClicked()
		{
			partyTab.Refresh();
			afterRefreshPartyTab.CheckAndCall();
		}

		public void OnDeckTabClicked()
		{
			deckTab.Refresh();
		}
	}
}