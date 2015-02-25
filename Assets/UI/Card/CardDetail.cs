using UnityEngine;
using UnityEngine.UI;

namespace Choanji.UI
{
	public class CardDetail : MonoBehaviour
	{
		public Image illust;
		public new Text name;
		public Image usage;
		public Image eleIcon;
		public Text eleName;
		public Text detail;

		public void SetCard(Card _card, CardMode? _mode)
		{
			var _data = _card.data;
			illust.sprite = R.Spr.CARD_ILLUST_S(_data.key);
			name.text = _data.name;

			if (_mode.HasValue)
			{
				usage.gameObject.SetActive(true);
				usage.sprite = R.Spr.CARD_USAGE(_data.GetModeUsage(_mode.Value));
			}
			else
				usage.gameObject.SetActive(false);

			var _ele = ElementDB.Get(_data.ele);
			eleIcon.sprite = R.Spr.ELE_S(_ele);
			eleName.text = _ele.name;

			if (!_mode.HasValue)
			{
				detail.text = _data.detail;
			}
			else
			{
				if (_mode == CardMode.ACTIVE)
					detail.text = _data.active.detail;
				else if (_mode == CardMode.PASSIVE)
					detail.text = _data.passive.detail;
			}
		}
	}
}