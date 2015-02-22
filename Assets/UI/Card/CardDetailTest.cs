#if UNITY_EDITOR

using UnityEngine;

namespace Choanji.UI
{

	public class CardDetailTest : MonoBehaviour
	{
		public CardDetail target;
		public bool useMode;
		public CardMode mode;

		void Start()
		{
			var _data = CardDB.Get(CardHelper.MakeID("KEYBOARD_WARRIOR"));
			target.SetCard(new Card(_data),  useMode ? mode : default(CardMode?));
		}
	}
}

#endif