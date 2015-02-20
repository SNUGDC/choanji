#if UNITY_EDITOR

using UnityEngine;

namespace Choanji.Battle
{
	public class CardViewTest : MonoBehaviour
	{
		public ActiveCardView activeCard;
		public PassiveCardView passiveCard;

		void Start()
		{
			var _data = CardDB.Get(CardHelper.MakeID("KEYBOARD_WARRIOR"));
			var _card = new Card(_data);

			activeCard.card.Setup(_card);
			activeCard.Setup(_data.active);

			passiveCard.card.Setup(_card);
			passiveCard.Setup(_data.passive);
		}
	}
}

#endif