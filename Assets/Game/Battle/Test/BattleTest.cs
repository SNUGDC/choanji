#if UNITY_EDITOR
using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	public class BattleTest : MonoBehaviour
	{
		void Start()
		{
			var _battlerA = new Setup.Battler()
			{
				baseStat = new StatSet(),
				party = new Party(),
			};
			
			var _battlerB = new Setup.Battler()
			{
				baseStat = new StatSet(),
				party = new Party(),
			};
			
			_battlerA.party.actives.Add(new Card(CardDB.Get(CardHelper.MakeID("KEYBOARD_WARRIOR"))));
			_battlerB.party.actives.Add(new Card(CardDB.Get(CardHelper.MakeID("LOGICAL_CRITICS"))));

			TheBattle.Setup(new Setup(Mode.SIM, _battlerA, _battlerB));

			TheBattle.battle.cardStartDelegate = (_battler, _card, _done) => _done(PhaseDoneType.CONTINUE);

			TheBattle.battle.onTurnEnd = () => Timer.g.Add(0, TheBattle.battle.SelectCards);

			TheBattle.Start();
		}
	}
}

#endif