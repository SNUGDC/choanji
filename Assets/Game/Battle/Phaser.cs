using System;

namespace Choanji.Battle
{
	public enum Phase
	{
		NONE,
		BUFF,
		SPE,
		ATK,
	}

	public class PhaserDelegate
	{
		public Action<Battler, Card, Action<bool>> startBuff;
		public Action<Battler, Card, Action<bool>> startSpe;
		public Action<Battler, Card, Action<bool>> startAtk;
	}

    public class Phaser
    {
		public bool isRunning { get { return phase != Phase.NONE; } }
		public Phase phase { get; private set; }
		public State battler { get; private set; }

		private readonly PhaserDelegate mDelegate;

	    public Phaser(State _state, PhaserDelegate _report)
		{
			battler = _state;
			mDelegate = _report;
		}

	    public void Go()
	    {
		    
	    }
	}
}
