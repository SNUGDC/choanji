#if UNITY_EDITOR

using UnityEngine;

namespace Choanji.UI
{
	public class DeckCellTest : MonoBehaviour
	{
		public DeckCell cell;
		public CardDetail detail;

		public bool hidden;
		public string card;

		void Start()
		{
			cell.canSelectPassive = () => true;
			cell.canSelectActive = () => true;

			cell.equipRequest = (_card, _mode, _confirm) => _confirm(true);
			cell.unequipRequest = (_card, _confirm) => _confirm(false);

			if (detail)
			{
				cell.onPointerHover = (_card, _mode, _enter) =>
				{
					if (_enter) detail.SetCard(_card, _mode);
				};
			}

			if (hidden)
			{
				cell.Hide();
			}
			else
			{
				var _data = CardDB.Get(CardHelper.MakeID(card));
				cell.Show(new Card(_data));
			}
		}
	}
}

#endif