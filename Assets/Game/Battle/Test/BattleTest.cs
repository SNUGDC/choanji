﻿#if UNITY_EDITOR
using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	public class BattleTest : MonoBehaviour
	{
		public EnvType env;
		public Mode mode;

		void Start()
		{
			var _battlerA = BattlerDB.Get(BattlerHelper.MakeID("SAMPLE_01"));
			var _battlerB = BattlerDB.Get(BattlerHelper.MakeID("SAMPLE_02"));

			TheBattle.Setup(new Setup(mode, _battlerA, _battlerB) { env = env });

			// TheBattle.battle.onCardPerform = (_battler, _card, _result, _done) => _done();
			TheBattle.battle.onTurnEnd = TheBattle.battle.StartTurn;
			TheBattle.onFinish = _result => Debug.Log(_result.type);

			TheBattle.Start();
		}
	}
}

#endif