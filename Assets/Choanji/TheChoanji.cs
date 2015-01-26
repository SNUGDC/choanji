namespace Choanji
{
	public class TheChoanji : Singleton<TheChoanji>
	{
		void Update()
		{
			WorldProgress.Update();
		}

		void LateUpdate()
		{
			WorldProgress.LateUpdate();
		}

	}
}