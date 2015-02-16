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
			if (_popup)
			{
				_popup.onClose += onPopupClosed;
				onPopupOpened.CheckAndCall();
			}
		}

		public void OnBagClicked()
		{
			onPopupOpened.CheckAndCall();
		}

		public void OnSaveClicked()
		{
			onPopupOpened.CheckAndCall();
		}

		public void OnOptionClicked()
		{
			onPopupOpened.CheckAndCall();
		}
	}
}