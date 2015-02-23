using System;
using System.Collections.Generic;
using Gem;

namespace Choanji.Battle
{
	public enum PhaseResult
	{
		PERFORM,
		NO_AP,
		DONE,
	}

    public class Phaser
    {
		public bool isRunning { get { return mNext != -1; } }
		public bool isSetuped { get { return mCardsA != null; } }
		public State state { get; private set; }

	    private int mNext = -1;
	    private Invoker mSlower;

	    private List<Card> mCardsA;
		private List<Card> mCardsB;

		private readonly Action mDone;
		private readonly Action<Invoker> mPerform;

		public Phaser(State _state, Action _done, Action<Invoker> _perform)
		{
			state = _state;
			mDone = _done;
			mPerform = _perform;
		}

	    public void Setup(List<Card> _cardsA, List<Card> _cardsB)
	    {
		    if (isRunning)
		    {
			    L.W("trying to setup again. ignore.");
			    return;
		    }

			mNext = 0;
			mCardsA = _cardsA;
		    mCardsB = _cardsB;
	    }

		public void Next()
	    {
			if (!isRunning)
			{
				L.E("not running.");
				return;
			}

			if (mSlower != null)
			{
				Do(mSlower);
				mSlower = null;
				return;
			}

			var _cur = mNext++;

			Invoker _invokerA = null;
			Invoker _invokerB = null;

			if (_cur < mCardsA.Count)
			{
				var _cardA = mCardsA[_cur];
				var _battlerA = state.battlerA;
				_invokerA = new Invoker(_battlerA, _cardA, CardMode.ACTIVE);
			}

			if (_cur < mCardsB.Count)
			{
				var _cardB = mCardsB[_cur];
				var _battlerB = state.battlerB;
				_invokerB = new Invoker(_battlerB, _cardB, CardMode.ACTIVE);
			}

			var _aOK = _invokerA != null;
			var _bOK = _invokerB != null;

			if (_aOK && _bOK)
				Do(_invokerA, _invokerB);
			else if (_aOK)
				Do(_invokerA);
			else if (_bOK)
				Do(_invokerB);
			else
				Done();
		}

		private static bool CheckEnoughAPAndDigest(Invoker _invoker)
		{
			var _cost = _invoker.card.data.active.cost;
			var _enough = _invoker.battler.ap >= _cost;
			if (!_enough)
				TheBattle.digest.Enq(new PhaserDigest(_invoker, PhaseResult.NO_AP));
			return _enough;
		}

	    private void Do(Invoker _invoker)
	    {
			if (CheckEnoughAPAndDigest(_invoker))
				Perform(_invoker);
		    else
				Done();
		}

		private void Do(Invoker _invokerA, Invoker _invokerB)
		{
			if (!CheckEnoughAPAndDigest(_invokerA))
		    {
				Do(_invokerB);
				return;
			}

			if (!CheckEnoughAPAndDigest(_invokerB))
		    {
				Do(_invokerA);
			    return;
		    }

			var _spdA = _invokerA.battler.CalStat(StatType.SPD);
			var _spdB = _invokerB.battler.CalStat(StatType.SPD);

		    Invoker _faster;
			Invoker _slower;

		    if (_spdA >= _spdB)
		    {
			    _faster = _invokerA;
				_slower = _invokerB;
		    }
		    else
		    {
				_faster = _invokerB;
				_slower = _invokerA;
		    }

		    Perform(_faster);

			if (CheckEnoughAPAndDigest(_slower))
				mSlower = _slower;
		}

	    private void Perform(Invoker _invoker)
	    {
			TheBattle.digest.Enq(new PhaserDigest(_invoker, PhaseResult.PERFORM));
		    mPerform(_invoker);
	    }

		private void Done()
	    {
			if (!isRunning)
			{
				L.E("already done.");
				return;
			}

		    mNext = -1;
			TheBattle.digest.Enq(new PhaserDigest(null, PhaseResult.DONE));
			mDone();
	    }
	}
}
