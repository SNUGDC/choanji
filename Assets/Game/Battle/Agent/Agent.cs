using System;
using System.Collections.Generic;
using Gem;

namespace Choanji.Battle
{
	public struct CardSelectYield
	{
		public readonly List<Card> cards;

		public CardSelectYield(List<Card> _cards)
		{
			cards = _cards;
		}
	}
	
	public class AgentReport
	{
		public AgentReport(Action<CardSelectYield> _cardSelectEnd)
		{
			cardSelectEnd = _cardSelectEnd;
		}

		public Action<CardSelectYield> cardSelectEnd;
	}

	public enum AgentType
	{
		ME, YOU, AI,
	}

	public abstract class Agent
	{
		public enum Job
		{
			NONE,
			CARD_SELECT,
		}

		public AgentType type { get; private set; }

		public Job job { get; private set; }
		public bool isRunning { get { return job != Job.NONE; } }

		public AgentReport report;

		protected Agent(AgentType _type)
		{
			type = _type;
		}

		public void StartCardSelect()
		{
			if (isRunning)
			{
				L.E("running " + job);
				return;
			}

			job = Job.CARD_SELECT;
			DoStartCardSelect();
		}

		public void EndCardSelect(CardSelectYield _yield)
		{
			if (job != Job.CARD_SELECT)
			{
				L.E("currect job is not card select.");
				return;
			}

			job = Job.NONE;
			report.cardSelectEnd(_yield);
		}

		protected abstract void DoStartCardSelect();
	}
}