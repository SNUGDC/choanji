using System;
using System.Collections.Generic;
using Choanji.UI;
using Gem;

namespace Choanji.Battle
{
	public static class Scene 
	{
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
