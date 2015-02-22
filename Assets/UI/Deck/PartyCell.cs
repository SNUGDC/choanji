using System;
using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji.UI
{
	public class PartyCell : MonoBehaviour
	{
		public Image illust;
		public GameObject detailRoot;
		public Text cost;
		public Text str;
		public Text def;
		public Text spd;

		public Action onCancel;

		public Card card
		{
			set
			{
				if (value == null)
				{
					illust.sprite = null;
					detailRoot.SetActive(false);
				}
				else
				{
					var _data = value.data;

					illust.sprite = R.BattleUI.Spr.CARD_ILLUST_S(_data.key);
					cost.text = _data.active.cost.ToString();

					var _stat = _data.stat;
					str.text = _stat.str.ToString();
					def.text = _stat.def.ToString();
					spd.text = _stat.spd.ToString();	
				}
			}
		}

		public void OnClickUp()
		{
			if (Input.GetMouseButtonUp(2))
			{
				if (detailRoot.activeSelf)
					onCancel.CheckAndCall();
			}
		}
	}
}