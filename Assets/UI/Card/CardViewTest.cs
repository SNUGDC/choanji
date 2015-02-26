#if UNITY_EDITOR

using UnityEngine;

namespace Choanji.UI
{
	public class CardViewTest : MonoBehaviour
	{
		public CardView target;
		public string card;
		public CardMode mode;

		void Start()
		{
			if (!target)
				target = FindObjectOfType<CardView>();

			if (!string.IsNullOrEmpty(card))
				target.SetCard(new Card(CardDB.Get(CardHelper.MakeID(card))), mode);
			else
				target.Clear();
		}
	}
}

#endif