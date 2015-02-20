using System;
using System.Collections.Generic;
using Choanji.UI;
using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	public class Scene : MonoBehaviour
	{
		private const float CARD_X_MARGIN = 0.2f;
		private const float CARD_Y_MARGIN = 0.02f;
		private const float CARD_HEIGHT = 0.3f;
		private const float CARD_WIDTH = 1 / (6 + CARD_X_MARGIN * 7);

		public Canvas parent;

		public HPBar hp;
		public APBar ap;

		public FieldView field;
		public BattlerView battler;

		void Start()
		{
			TheBattle.onSetup += Setup;
		}

		void OnDestroy()
		{
			TheBattle.onSetup -= Setup;
		}

		public void Setup(Setup _setup)
		{
			var _battlerA = TheBattle.state.battlerA;
			var _battlerB = TheBattle.state.battlerB;

			hp.max = (int)_battlerA.hpMax;
			ap.max = (int)_battlerA.apMax;
			_battlerA.onHPMod += (_cur, _old) => hp.Set((int)_cur);
			_battlerA.onAPMod += (_cur, _old) => ap.Set((int)_cur);

			SetupCards();

			field.env = _setup.env;
			battler.SetBattler(_battlerB.data.key);
		}

		private void SetupCards()
		{
			var _partyA = TheBattle.state.battlerA.party;

			var i = 0;

			foreach (var _card in _partyA.actives)
			{
				var _view = PrefabDB.g.activeCardView.Instantiate();
				PositionCard((RectTransform)_view.transform, i);
				_view.card.Setup(_card.data);
				_view.Setup(_card.data.active);
				++i;
			}

			foreach (var _card in _partyA.passives)
			{
				var _view = PrefabDB.g.passiveCardView.Instantiate();
				PositionCard((RectTransform)_view.transform, i);
				_view.card.Setup(_card.data);
				_view.Setup(_card.data.passive);
				++i;
			}
		}

		private void PositionCard(RectTransform _rect, int _idx)
		{
			var _size = new Vector2(CARD_WIDTH, CARD_HEIGHT);
			_rect.SetParent(parent.transform);
			_rect.offsetMin = Vector2.zero;
			_rect.offsetMax = Vector2.zero;
			_rect.anchorMin = new Vector2(CARD_WIDTH * (CARD_X_MARGIN * (_idx + 1) + _idx), CARD_Y_MARGIN);
			_rect.anchorMax = _size + _rect.anchorMin;
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
