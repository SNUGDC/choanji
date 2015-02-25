using System;
using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji.UI
{
	public class CardView : MonoBehaviour
	{
		public Card card { get; private set; }

		public new Text name;
		public Image panel;
		public Image illust;

		public Image usage;
		public Image ele;
		public Text move;

		public Text cost;
		public Text stat;

		public Action onSelect;
		public Action onCancel;
		public Action onShowDetail;
		public Action<bool> onPointerOver;

		public void SetCard(Card _card, CardMode _mode)
		{
			card = _card;

			var _data = _card.data;

			name.text = _data.name;
			illust.sprite = R.Spr.CARD_ILLUST_S(_data.key);
			
			usage.sprite = R.Spr.CARD_USAGE(_data.GetModeUsage(_mode));
			ele.sprite = R.Spr.ELE_S(_data.ele);
			move.text = _data.GetModeName(_mode);

			var _stat = _data.stat;
			stat.text = _stat.str + " / " + _stat.def + " / " + _stat.spd;

			if (_mode == CardMode.ACTIVE)
			{
				var _move = _data.active;
				panel.color = UnityHelper.ColorOrDefault(_move.theme, ActiveData.DEFAULT_THEME);
				cost.text = _move.cost.ToString();
			}
			else
			{
				var _move = _data.passive;
				panel.color = UnityHelper.ColorOrDefault(_move.theme, PassiveData.DEFAULT_THEME);
				cost.text = "-";
			}
		}

		public void Clear()
		{
			card = null;
			name.text = "";
			illust.sprite = null;
			usage.sprite = null;
			ele.sprite = null;
			move.text = "";
			stat.text = "- / - / -";
			cost.text = "-";
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