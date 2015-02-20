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

	public partial class Battle
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

		public Action<Result, Action> endDelegate;

		public Action<Battler, Card, PerformResult, Action> onCardPerform;
		public Action onTurnEnd;
		public Action<Result> onFinish;

		public Battle(Mode _mode, State _state)
		{
			L.SetLevel(L.V.D);

			mode = _mode;
			state = _state;

			var _agents = Helper.ModeToAgents(mode);
			mAgentA = new AgentInfo(AgentFactory.Make(_agents.first, _state.battlerA));
			mAgentB = new AgentInfo(AgentFactory.Make(_agents.second, _state.battlerB));

			mAgentA.agent.report = new AgentReport(_cards => AssignCardsAndProceed(mAgentA, _cards));
			mAgentB.agent.report = new AgentReport(_cards => AssignCardsAndProceed(mAgentB, _cards));

			mPhase = new Phaser(_state, OnPhaseDone, new PhaserDelegate(PerformActive));
		}

		public void SelectCards()
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

		private void PerformActive(Battler _battler, Card _card, Action<PhaseDoneType> _done)
		{
			L.D("perform");

			var _result = PerformActive(_battler, _card);

			PhaseDoneType _doneType;

			if (state.battlerA.hp <= 0)
				_doneType = PhaseDoneType.WIN_B;
			else if (state.battlerB.hp <= 0)
				_doneType = PhaseDoneType.WIN_A;
			else
				_doneType = PhaseDoneType.CONTINUE;

			onCardPerform(_battler, _card, _result, () => _done(_doneType));
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
					if (endDelegate != null)
						endDelegate(_result, () => onFinish(_result));
					else
						onFinish(_result);
					break;
				}
				case PhaseDoneType.WIN_B:
				{
					var _result = new Result(ResultType.WIN_B);
					if (endDelegate != null)
						endDelegate(_result, () => onFinish(_result));
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
