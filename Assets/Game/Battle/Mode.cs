using Gem;

namespace Choanji.Battle
{
	using AgentPair = Pair<AgentType, AgentType>;

	public enum Mode
	{
		PVE, PVP, SIM, 
	}

	public static partial class Helper
	{
		public static AgentPair ModeToAgents(Mode _mode)
		{
			switch (_mode)
			{
				case Mode.PVE: return new AgentPair(AgentType.ME, AgentType.AI);
				case Mode.PVP: return new AgentPair(AgentType.ME, AgentType.YOU);
				case Mode.SIM: return new AgentPair(AgentType.AI, AgentType.AI);
				default: D.Assert(false); return new AgentPair(AgentType.AI, AgentType.AI);
			}
		}
	}
}