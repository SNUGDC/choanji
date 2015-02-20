#if UNITY_EDITOR
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
			var _battlerB = BattlerDB.Get(BattlerHelper.MakeID("SAMPLE_01"));

			TheBattle.Setup(new Setup(mode, _battlerA, _battlerB) { env = env });

			TheBattle.battle.onCardPerform = (_battler, _card, _result, _done) => _done();
			TheBattle.battle.onTurnEnd = () => Timer.g.Add(0, TheBattle.battle.SelectCards);
			TheBattle.battle.onFinish = _result => Debug.Log(_result.type);

			TheBattle.Start();
		}
	}
}

#endif