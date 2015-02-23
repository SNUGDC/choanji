using System;
using System.Collections.Generic;
using Gem;

namespace Choanji.Battle
{
	public enum BattlerPosition { A, B }

	public static partial class Helper
	{
		public static BattlerPosition Other(BattlerPosition _pos)
		{
			return (_pos == BattlerPosition.A)
				? BattlerPosition.B
				: BattlerPosition.A;
		}
	}

	public class Battle
	{
		private class AgentInfo
		{
			public AgentInfo(Agent _agent)
			{
				agent = _agent;
			}

			public readonly Agent agent;
			public List<Card> selectedCards;
		}

		public Mode mode { get; private set; }
		public State state { get; private set; }

		private readonly AgentInfo mAgentA;
		private readonly AgentInfo mAgentB;

		private readonly Phaser mPhase;
		private ResultType? mResult;

		public Action onTurnStart;
		public Action onTurnEnd;
		public Action<Result> onFinish;

		public Battle(Mode _mode, State _state)
		{
			mode = _mode;
			state = _state;

			var _agents = Helper.ModeToAgents(mode);
			mAgentA = new AgentInfo(AgentFactory.Make(_agents.first, _state.battlerA));
			mAgentB = new AgentInfo(AgentFactory.Make(_agents.second, _state.battlerB));

			mAgentA.agent.report = new AgentReport(_cards => AssignCards(mAgentA, _cards));
			mAgentB.agent.report = new AgentReport(_cards => AssignCards(mAgentB, _cards));

			mPhase = new Phaser(_state, EndTurn, PerformActive);

			foreach (var _card in state.battlerA.party.passives)
				AddPassiveTA(state.battlerA, _card);
			foreach (var _card in state.battlerB.party.passives)
				AddPassiveTA(state.battlerB, _card);
		}
		
		private ResultType? CheckDone()
		{ 
			if (!state.battlerA.alive)
				return ResultType.WIN_B;
			if (!state.battlerB.alive)
				return ResultType.WIN_A;
			return null;
		}

		public void Update()
		{
			if (mResult.HasValue)
				return;

			mResult = CheckDone();

			if (mResult.HasValue)
				Finish();
			else if (!mPhase.isRunning)
				TryStartPhase();
			else
				mPhase.Next();
		}

		private static void AddPassiveTA(Battler _battler, Card _card)
		{
			var _invoker = new Invoker(_battler, _card, CardMode.PASSIVE);
			var _digest = new PassiveFireDigest(_invoker);

			var _ta = _card.data.passive.perform;

			if (_ta.trigger != null)
			{
				var _fakeTA = new TA(_ta.trigger, new ActionLambda((_invoker2, _arg) => 
				{
					TheBattle.digest.Enq(_digest);
					return _ta.action.Invoke(_invoker2, _arg);
				}));

				TheBattle.trigger.Add(_invoker, _fakeTA);
			}
			else
			{
				TheBattle.digest.Enq(_digest);
				TheBattle.trigger.Fire(_invoker, _ta.action, null);
			}
		}

		public void StartTurn()
		{
			onTurnStart.CheckAndCall();
			TheBattle.digest.Enq(new TypedDigest(null, DigestType.BATTLE_TURN_START));
			state.battlerA.AfterTurnEnd();
			state.battlerB.AfterTurnEnd();
			SelectCards();
		}

		private void EndTurn()
		{
			state.battlerA.RegenAP();
			state.battlerB.RegenAP();
			onTurnEnd.CheckAndCall();
		}

		private void SelectCards()
		{
			mAgentA.agent.StartCardSelect();
			mAgentB.agent.StartCardSelect();
		}

		private void AssignCards(AgentInfo _agent, CardSelectYield _yield)
		{
			D.Assert(_yield.cards != null);
			_agent.selectedCards = _yield.cards;
		}

		private void TryStartPhase()
		{
			if (mAgentA.selectedCards != null && mAgentB.selectedCards != null)
				mPhase.Setup(mAgentA.selectedCards, mAgentB.selectedCards);
		}

		private void PerformActive(Invoker _invoker)
		{
			var _active = _invoker.card.data.active;
			var _perform = _active.perform;

			_invoker.battler.ConsumeAP(_active.cost);

			if (_perform.trigger == null)
				TheBattle.trigger.Fire(_invoker, _perform.action, null);
			else
				TheBattle.trigger.Add(_invoker, _perform);
		}

		private void Finish()
		{
			if (!mResult.HasValue)
			{
				L.E("result doesn't have value.");
				return;
			}

			mAgentA.selectedCards = null;
			mAgentB.selectedCards = null;
			onFinish(new Result(mResult.Value));
		}

	}
}
