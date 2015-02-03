using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji
{
	public class CardView : MonoBehaviour
	{
		public CardData data { get; private set; }

		public Text name_;
		public Text detail;

		public SpriteRenderer iconMode;

		public bool isSetuped { get { return data != null; } }

		public void Setup(CardData _data)
		{
			if (isSetuped)
			{
				L.E(L.M.CALL_RETRY("setup"));
				return;
			}

			data = _data;
			name_.text = _data.name;
			detail.text = _data.detail;
		}

		public void SetMode(CardMode _mode)
		{
			if (!isSetuped)
			{
				L.E(L.M.STATE_INVALID);
				return;
			}

			iconMode.sprite = Resources.Load<Sprite>(R.Game.Spr.CardIcon(_mode));
		}
	}
}