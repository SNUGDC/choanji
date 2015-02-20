using System;
using System.Collections.Generic;
using Gem;

namespace Choanji.Battle
{
	public enum PhaseDoneType
	{
		CONTINUE,
		TURN_END,
		WIN_A,
		WIN_B,
	}

	public class PhaserDelegate
	{
		public PhaserDelegate(Action<Battler, Card, Action<PhaseDoneType>> _start)
		{
			start = _start;
		}
		
		public Action<Battler, Card, Action<PhaseDoneType>> start;
	}

    public class Phaser
    {
		public bool isRunning { get { return mRunning != -1; } }
		public State state { get; private set; }

		public readonly Action<PhaseDoneType> onDone;

	    private int mRunning = -1;
	    private List<Card> mCardsA;
		private List<Card> mCardsB;
		private readonly PhaserDelegate mDelegate;

		public Phaser(State _state, Action<PhaseDoneType> _onDone, PhaserDelegate _delegate)
		{
			state = _state;
		    onDone = _onDone;
			mDelegate = _delegate;
		}

	    public void Start(List<Card> _cardsA, List<Card> _cardsB)
	    {
		    if (isRunning)
		    {
			    L.W("trying to go again. ignore.");
			    return;
		    }

		    mCardsA = _cardsA;
		    mCardsB = _cardsB;

			Loop();
	    }

	    private void Loop()
	    {
			++mRunning;

			if (mRunning >= mCardsA.Count && mRunning >= mCardsB.Count)
				Done(PhaseDoneType.TURN_END);
			else if (mRunning >= mCardsA.Count)
				Go(state.battlerB, mCardsB);
			else if (mRunning >= mCardsB.Count)
				Go(state.battlerA, mCardsA);
			else 
				Go();
	    }

	    private void Go(Battler _battler, List<Card> _cards)
		{
			mDelegate.start(_battler, _cards[mRunning], _doneType =>
			{
				if (_doneType == PhaseDoneType.CONTINUE)
				{
					if (mRunning >= _cards.Count)
						Done(_doneType);
					else
					{
						++mRunning;
						if (mRunning < _cards.Count)
							Go(_battler, _cards);
						else
							Done(PhaseDoneType.TURN_END);
					}
				}
				else
				{
					Done(_doneType);
				}
			});
	    }

	    private void Go()
	    {
			var _cardA = mCardsA[mRunning];
			var _cardB = mCardsB[mRunning];

		    var _battlerA = state.battlerA;
		    var _battlerB = state.battlerB;
			var _spdA = _battlerA.CalStat(StatType.SPD);
			var _spdB = _battlerB.CalStat(StatType.SPD);

		    Battler _faster;
			Battler _slower;
		    Card _fasterCard;
		    Card _slowerCard;

		    if (_spdA > _spdB)
		    {
			    _faster = _battlerA;
			    _slower = _battlerB;
			    _fasterCard = _cardA;
			    _slowerCard = _cardB;
		    }
		    else
		    {
				_faster = _battlerB;
				_slower = _battlerA;
				_fasterCard = _cardB;
				_slowerCard = _cardA;
		    }

			mDelegate.start(_faster, _fasterCard, _doneTypeA =>
			{
				if (_doneTypeA == PhaseDoneType.CONTINUE)
					mDelegate.start(_slower, _slowerCard, _doneTypeB =>
					{
						if (_doneTypeB == PhaseDoneType.CONTINUE)
							Loop();
						else
							Done(_doneTypeB);
					});
				else
					Done(_doneTypeA);
			});
	    }

		private void Done(PhaseDoneType _doneType)
	    {
		    mRunning = -1;
		    onDone(_doneType);
	    }
	}
}
