using Gem;
using UnityEngine;

namespace Choanji
{
	public class TheChoanji : Singleton<TheChoanji>
	{
		public ContextType context;

		void Update()
		{
			Timer.g.Update(Time.deltaTime);
			
			switch (context)
			{
				case ContextType.INTRO:
					break;
				case ContextType.WORLD:
					WorldProgress.Update();
					break;
			}
		}

		void LateUpdate()
		{
			switch (context)
			{
				case ContextType.INTRO:
					break;
				case ContextType.WORLD:
					WorldProgress.LateUpdate();
					break;
			}
		}

	}
}