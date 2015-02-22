#if UNITY_EDITOR
using Choanji.Battle;
using UnityEngine;

namespace Choanji.UI
{
	public class DeckPopupTest : MonoBehaviour
	{
		public DeckPopup target;

		void Start()
		{
			var _battler = BattlerDB.Get(BattlerHelper.MakeID("SAMPLE_01"));
			target.partyTab.stat = _battler.stat;
			target.partyTab.party = _battler.party;
		}
	}
}

#endif