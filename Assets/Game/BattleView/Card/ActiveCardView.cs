using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji.Battle
{
	public class ActiveCardView : MonoBehaviour
	{
		public CardView card;

		public Text name_;
		public Text cost;
		public Image iconType;
		public Image iconEle;

		public void Setup(ActiveData _data)
		{
			name_.text = _data.name;

			cost.text = _data.cost.ToString();

			iconType.sprite = R.BattleUI.Spr.CARD_ACTIVE_TYPE(_data.type);

			if (_data.perform.id == ActivePerform.ID.DMG)
				iconEle.sprite = R.Spr.ELE_S(((ActivePerform.Dmg)_data.perform).ele);
			else 
				iconEle.gameObject.SetActive(false);
		}
	}

}
