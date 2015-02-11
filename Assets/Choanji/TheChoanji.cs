using Gem;
using UnityEngine;

namespace Choanji
{
	public class TheChoanji : Singleton<TheChoanji>
	{
		void Update()
		{
			Timer.g.Update(Time.deltaTime);
			WorldProgress.Update();
		}

		void LateUpdate()
		{
			WorldProgress.LateUpdate();
		}

	}
}