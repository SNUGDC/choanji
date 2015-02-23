using System.Diagnostics;

namespace Choanji.Battle
{
	[DebuggerDisplay("{mDebuggerDisplay}")]
    public class State
    {
		public readonly Battler battlerA;
		public readonly Battler battlerB;

		public State(Battler _battlerA, Battler _battlerB)
        {
			battlerA = _battlerA;
			battlerB = _battlerB;
        }

		public bool IsA(Battler _battler)
		{
			return battlerA == _battler;
		}

		public bool IsB(Battler _battler)
		{
			return battlerB == _battler;
		}

	    public Battler Other(Battler _battler)
	    {
		    return IsA(_battler) ? battlerB : battlerA;
	    }

		private string mDebuggerDisplay
		{
			get
			{
				return "BattlerA: " + battlerA.hp + "\n"
					+ "BattlerB: " + battlerB.hp;
			}
		}
    }
}
