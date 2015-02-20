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

			var _passive = new PassiveData("LIGHTNING_ROD", "피뢰침", "설명");
			var _active = new ActiveData("SPARK", ActiveType.ATK, "스파크", "설명", 50,
				new ActivePerform.Dmg(ElementDB.Search("ELE"), 40));
			var _cardData = new CardData("CIRCUIT_II", "회로2", "설명", new StatSet(), _passive, _active);

			_battlerA.party.actives.Add(new Card(_cardData));
			_battlerB.party.actives.Add(new Card(_cardData));

			TheBattle.Setup(new Setup(Mode.SIM, _battlerA, _battlerB));

			TheBattle.battle.cardStartDelegate = (_battler, _card, _done) => _done(PhaseDoneType.CONTINUE);

			TheBattle.battle.onTurnEnd = () => Timer.g.Add(0, TheBattle.battle.SelectCards);

			TheBattle.Start();
		}
	}
}

#endif