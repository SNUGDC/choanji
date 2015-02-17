using Gem;

namespace Choanji.Battle
{
	public static class AgentFactory 
	{
		public static Agent Make(AgentType _type)
		{
			switch (_type)
			{
				case AgentType.ME:   return new MyAgent();
				case AgentType.YOU:  return new YourAgent();
				case AgentType.AI:   return new AIAgent();
				default:
					D.Assert(false);
					return new AIAgent();
			}
		}
	}
}