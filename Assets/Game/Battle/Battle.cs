using System;
using System.Collections.Generic;

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
		private readonly State mState;

		private class AgentInfo
		{
			public AgentInfo(Agent _agent)
			{
				agent = _agent;
			}

			public readonly Agent agent;
			public List<Card> selectedCards;
		}

		private readonly Mode mode;
		private readonly AgentInfo mAgentA;
		private readonly AgentInfo mAgentB;

		private readonly Phaser mPhase;

		public Action<Result, Action> endDelegate;

		public Battle(Mode _mode, State _state)
		{
			mode = _mode;

			mState = _state;
			
			var _agents = Helper.ModeToAgents(mode);
			mAgentA = new AgentInfo(AgentFactory.Make(_agents.first));
			mAgentB = new AgentInfo(AgentFactory.Make(_agents.second));

			mAgentA.agent.report = new AgentReport(_cards => AssignCardAndProceed(mAgentA, _cards));
			mAgentB.agent.report = new AgentReport(_cards => AssignCardAndProceed(mAgentB, _cards));

			mPhase = new Phaser(_state, new PhaserDelegate());
		}

		public void Go()
		{
			var _statA = mState.battlerA.party.CalStat();
			var _statB = mState.battlerB.party.CalStat();

			var _faster = (_statA.spd > _statB.spd) ? mAgentA : mAgentB;
			_faster.agent.StartCardSelect();
		}

		private void AssignCardAndProceed(AgentInfo _agent, CardSelectYield _selected)
		{
			_agent.selectedCards = _selected.cards;

			var _other = (_agent == mAgentA) 
				? mAgentB : mAgentA;

			if (_other.selectedCards == null)
				_other.agent.StartCardSelect();
			else
				StartPhase();
		}

		private void StartPhase()
		{
			mPhase.Go();
		}

		/*
		private ResultType CheckEndTurn()
		{
			ResultType _endType;

			if (mState.player.Hp <= 0)
			{
				_endType = ResultType.LOSE;
			}
			else if (mState.enemy.Hp <= 0)
			{
				_endType = ResultType.WIN;
			}
			else
			{
				_endType = ResultType.NOTEND;
			}
			return _endType;
		}
		*/
		/*
		public void End(ResultType _type)
		{
			if (endDelegate == null)
				endDelegate();
			else
				endDelegate();
		}
		 */
	}
}
