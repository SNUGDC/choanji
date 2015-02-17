namespace Choanji.Battle
{
	public enum BattlerID { }

	public class Battler
	{
		public Battler(StatSet _baseStat, Party _party)
		{
			baseStat = _baseStat;
			partyStat = _party.CalStat();

			hpMax = hp = baseStat.hp + (int)partyStat.hp;
			apMax = ap = baseStat.ap + (int)partyStat.ap;

			party = _party;
		}

		public readonly StatSet baseStat;
		public readonly StatSet partyStat;
		public readonly StatSet dynamicStat = new StatSet();

		public readonly HP hpMax;
		public HP hp { get; private set; }

		public readonly AP apMax;
		public AP ap { get; private set; }

		public readonly Party party;
	}
}