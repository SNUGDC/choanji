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

		public Action<ActionInvoker, ActionResult, Action> requestProceedActive;
		public Action<Result, Action> requestFinish;

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

			mAgentA.agent.report = new AgentReport(_cards => AssignCardsAndProceed(mAgentA, _cards));
			mAgentB.agent.report = new AgentReport(_cards => AssignCardsAndProceed(mAgentB, _cards));

			mPhase = new Phaser(_state, OnPhaseDone, new PhaserDelegate(PerformActive));

			foreach (var _card in state.battlerA.party.passives)
				AddPassiveTA(state.battlerA, _card);
			foreach (var _card in state.battlerB.party.passives)
				AddPassiveTA(state.battlerB, _card);
		}

		private void AddPassiveTA(Battler _battler, Card _card)
		{
			var _invoker = new ActionInvoker(_battler, _card);
			var _ta = _card.data.passive.perform;
			if (_ta.trigger != null)
				TheBattle.trigger.Add(_invoker, _ta);
			else
				TheBattle.action.Fire(_invoker, _ta.action, null);
		}

		public void StartTurn()
		{
			onTurnStart.CheckAndCall();

			state.battlerA.AfterTurnEnd();
			state.battlerB.AfterTurnEnd();

			SelectCards();
		}

		private void SelectCards()
		{
			mAgentA.agent.StartCardSelect();
			mAgentB.agent.StartCardSelect();
		}

		private void AssignCardsAndProceed(AgentInfo _agent, CardSelectYield _yield)
		{
			D.Assert(_yield.cards != null);

			_agent.selectedCards = _yield.cards;

			var _other = (_agent == mAgentA) 
				? mAgentB : mAgentA;

			if (_other.selectedCards != null)
				StartPhase();
		}

		private void StartPhase()
		{
			mPhase.Start(
				mAgentA.selectedCards, 
				mAgentB.selectedCards);
		}

		private void PerformActive(ActionInvoker _invoker, Action<PhaseDoneType> _done)
		{
			ActionResult _result = null;

			var _perform = _invoker.card.data.active.perform;

			if (_perform.trigger == null)
				_result = TheBattle.action.Fire(_invoker, _perform.action, null);
			else
				TheBattle.trigger.Add(_invoker, _perform);

			PhaseDoneType _doneType;

			if (state.battlerA.hp <= 0)
				_doneType = PhaseDoneType.WIN_B;
			else if (state.battlerB.hp <= 0)
				_doneType = PhaseDoneType.WIN_A;
			else
				_doneType = PhaseDoneType.CONTINUE;

			requestProceedActive(_invoker, _result, () => _done(_doneType));
		}

		private void OnPhaseDone(PhaseDoneType _doneType)
		{
			mAgentA.selectedCards = null;
			mAgentB.selectedCards = null;

			switch (_doneType)
			{
				case PhaseDoneType.TURN_END:
					OnTurnEnd();
					break;
				case PhaseDoneType.WIN_A:
				{
					var _result = new Result(ResultType.WIN_A);
					if (requestFinish != null)
						requestFinish(_result, () => onFinish(_result));
					else
						onFinish(_result);
					break;
				}
				case PhaseDoneType.WIN_B:
				{
					var _result = new Result(ResultType.WIN_B);
					if (requestFinish != null)
						requestFinish(_result, () => onFinish(_result));
					else
						onFinish(_result);
					break;
				}
				default:
					D.Assert(false);
					break;
			}
		}

		private void OnTurnEnd()
		{
			state.battlerA.RegenAP();
			state.battlerB.RegenAP();
			onTurnEnd();
		}
	}
}
