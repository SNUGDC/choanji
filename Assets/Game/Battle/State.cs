using System;
using System.Collections.Generic;

namespace Choanji.Battle
{
	public class BattlerState
	{
		public BattlerState()
		{
			foreach (var _ele in ElementDB.GetEnum())
				attackModifier.Add(_ele, 0);

			attackBuilder.Add(_org =>
			{
				int _mod;
				if (!attackModifier.TryGetValue(_org.ele, out _mod))
					return _org;
				_org.val = (HP)((float)_org.val * ((100 + _mod)/100f));
				return _org;
			});
		}

		public readonly Dictionary<ElementID, int> attackModifier = new Dictionary<ElementID, int>();
		public readonly DamageBuilder attackBuilder = new DamageBuilder();

		public bool blockHitOneTime;
		public Action<Damage> beforeHit;
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
