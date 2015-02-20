using System;
using System.Collections.Generic;
using Choanji.UI;
using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	public class Scene : MonoBehaviour
	{
		public HPBar hp;
		public APBar cost;

		void Start()
		{
			TheBattle.onSetup += Setup;
		}

		void OnDestroy()
		{
			TheBattle.onSetup -= Setup;
		}

		public void Setup()
		{
			var _battlerA = TheBattle.state.battlerA;
			hp.max = (int)_battlerA.hpMax;
			cost.max = (int)_battlerA.apMax;

			_battlerA.onHPMod += (_cur, _old) => hp.Set((int)_cur);
			_battlerA.onAPMod += (_cur, _old) => cost.Set((int)_cur);
		}

		public static void GatherPlayerCard(Action<List<Card>> _onDone)
		{
		}

		public static void AnimateBattleEnd(Result _result, Action _onDone)
		{
			Popup _popup = null;

			switch (_result.type)
			{
				case ResultType.WIN_A:
					_popup = ThePopup.Open(Popups.BATTLE_WIN);
					break;
				case ResultType.WIN_B:
					_popup = ThePopup.Open(Popups.BATTLE_LOSE);
					break;
			}

			if (_popup)
				_popup.onClose += _onDone;
		}
	}
}
