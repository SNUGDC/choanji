namespace Choanji.Battle
{
	public class MyAgent : Agent
	{
		public MyAgent() 
			: base(AgentType.ME)
		{}

		protected override void DoStartCardSelect()
		{
			Scene.GatherPlayerCard(_cards => EndCardSelect(new CardSelectYield(_cards)));
		}
	}
}