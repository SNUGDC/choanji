using System;

namespace Choanji.Battle
{
	public class BattlerState
	{
		public Action<Damage> beforeHit;
		public bool blockHitOneTime;
	}

    public class State
    {
		public readonly Battler battlerA;
		public readonly Battler battlerB;

	    public readonly BattlerState battlerAState = new BattlerState();
		public readonly BattlerState battlerBState = new BattlerState();
		
		public State(Battler _battlerA, Battler _battlerB)
        {
			battlerA = _battlerA;
			battlerB = _battlerB;
        }

	    public BattlerState GetStateOf(Battler _battler)
	    {
		    return (_battler == battlerA)
			    ? battlerAState
			    : battlerBState;
	    }

        public override string ToString()
        {
            return "BattlerA: " + battlerA.hp + "\n"
				+ "BattlerB: " + battlerB.hp;
        }
    }
}
