using Gem;
using UnityEngine;

namespace Choanji.UI
{
	public class DeckPopup : Popup
	{
		public PartyTab partyTab;

		public RectTransform cardDetailParent;

		[HideInInspector]
		public CardDetail cardDetail;

		void Start()
		{
			cardDetail = DB.g.cardDetailPrf.Instantiate();
			var _rect = cardDetail.gameObject.RectTransform();
			_rect.SetParent(cardDetailParent, false);
			_rect.Fill();
		}
	}
}