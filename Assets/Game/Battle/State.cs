using System;
using System.Collections.Generic;

namespace Choanji.Battle
{
    public class State
    {
		public readonly Battler battlerA;
		public readonly Battler battlerB;

		public State(Battler _battlerA, Battler _battlerB)
        {
			battlerA = _battlerA;
			battlerB = _battlerB;
        }

	    public Battler Other(Battler _battler)
	    {
		    return battlerA == _battler ? battlerB : battlerA;
	    }

        public override string ToString()
        {
            return "BattlerA: " + battlerA.hp + "\n"
				+ "BattlerB: " + battlerB.hp;
        }
    }
}
