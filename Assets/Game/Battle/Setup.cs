using UnityEngine;

namespace Choanji.Battle
{
	public class Setup
	{
		public Mode mode;

		public EnvType env;
		public int envIdx;

		public AudioClip bgm;

		public BattlerData battlerA;
		public BattlerData battlerB;

		public Setup(Mode _mode, BattlerData _battlerA, BattlerData _battlerB)
		{
			mode = _mode;
			battlerA = _battlerA;
			battlerB = _battlerB;
		}
	}
}
