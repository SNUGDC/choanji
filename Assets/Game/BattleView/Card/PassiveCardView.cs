using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Choanji.Battle
{
	public class PassiveCardView : MonoBehaviour
	{
		public CardView card;
		public Text name_;

		public void Setup(PassiveData _data)
		{
			name_.text = _data.name;
		}
	}


}