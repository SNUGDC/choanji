namespace Choanji.Battle
{
	public class MyAgent : Agent
	{
		public MyAgent() 
			: base(AgentType.ME)
		{}

		protected override void DoStartCardSelect()
		{
			Scene.g.GatherCards(_cards => EndCardSelect(new CardSelectYield(_cards)));
		}
	}
}