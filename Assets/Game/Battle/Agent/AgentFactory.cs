using Gem;

namespace Choanji.Battle
{
	public static class AgentFactory 
	{
		public static Agent Make(AgentType _type, Battler _battler)
		{
			switch (_type)
			{
				case AgentType.ME:   return new MyAgent();
				case AgentType.YOU:  return new YourAgent();
				case AgentType.AI:   return new AIAgent(_battler);
				default:
					D.Assert(false);
					return new AIAgent(_battler);
			}
		}
	}
}