using UnityEngine;

namespace Choanji
{
	public class PrefabDB : MonoBehaviour
	{
		public static PrefabDB g;
		public Character ch;
		public Battle.ActiveCardView activeCardView;
		public Battle.PassiveCardView passiveCardView;
		public Battle.SelectionCell selectedCardCell;
	}
}