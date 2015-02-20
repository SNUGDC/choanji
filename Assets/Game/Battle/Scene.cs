using System;
using System.Collections.Generic;
using Choanji.UI;
using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	public class Scene : MonoBehaviour
	{
		public static Scene g;

		public Canvas parent;

		public FieldView field;
		public BattlerView battler;

		public HPBar hp;
		public APBar ap;

		public PartyView party;

		public SubmitButton submit;

		void Awake()
		{
			g = this;
		}

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

			field.env = _setup.env;
			battler.SetBattler(_battlerB.data.key);

			hp.max = (int)_battlerA.hpMax;
			ap.max = (int)_battlerA.apMax;
			_battlerA.onHPMod += (_cur, _old) => hp.Set((int)_cur);
			_battlerA.onAPMod += (_cur, _old) => ap.Set((int)_cur);

			party.Setup(_battlerA.party);
		}

		public void GatherCards(Action<List<Card>> _onDone)
		{
			submit.onClick = () =>
			{
				_onDone(party.Submit());
				submit.onClick = null;
			};
		}

		public void AnimateBattleEnd(Result _result, Action _onDone)
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
