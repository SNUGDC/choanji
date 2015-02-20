namespace Choanji.Battle
{
	public class Setup
	{
		public struct Battler
		{
			public BattlerID id;
			public StatSet baseStat;
			public Party party;
		}

		public Mode mode;

		public EnvType env;
		public int envIdx;

		public Battler battlerA;
		public Battler battlerB;

		public Setup(Mode _mode, Battler _battlerA, Battler _battlerB)
		{
			mode = _mode;
			battlerA = _battlerA;
			battlerB = _battlerB;
		}
	}
}
