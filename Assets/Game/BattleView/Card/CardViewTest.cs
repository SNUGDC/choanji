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
			var _passive = new PassiveData("LIGHTNING_ROD", "피뢰침", "설명");
			var _active = new ActiveData("SPARK", ActiveType.ATK, "스파크", "설명", (AP)50,
				new ActivePerform.Dmg(ElementDB.Search("ELE"), (HP)40));
			var _data = new CardData("CIRCUIT_II", "회로2", "설명", new StatSet(), _passive, _active);
			var _card = new Card(_data);

			activeCard.card.Setup(_card);
			activeCard.Setup(_active);

			passiveCard.card.Setup(_card);
			passiveCard.Setup(_passive);
		}
	}
}

#endif