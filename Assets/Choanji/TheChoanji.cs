using Gem;
using UnityEngine;

namespace Choanji
{
	public class TheChoanji : Singleton<TheChoanji>
	{
		public ContextType context;

		void Update()
		{
			var _dt = Time.deltaTime;
			Timer.g.Update(_dt);
			
			switch (context)
			{
				case ContextType.INTRO:
					break;
				case ContextType.WORLD:
					UI.UI.Update(_dt);
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