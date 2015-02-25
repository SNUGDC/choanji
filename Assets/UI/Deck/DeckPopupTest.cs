#if UNITY_EDITOR
using UnityEngine;

namespace Choanji.UI
{
	public class DeckPopupTest : MonoBehaviour
	{
		public DeckPopup target;

		void Start()
		{
			Disket.LoadOrDefault("test");
			DisketHelper.SetupCommon();

			target.party = Player.party;
			target.deckTab.deck = Player.deck;
			target.partyTab.stat = Player.totalStat;
		}
	}
}

#endif