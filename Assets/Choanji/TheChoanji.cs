using Gem;
using UnityEngine;

namespace Choanji
{
	public class TheChoanji : Singleton<TheChoanji>
	{
		private ContextType mContext = ContextType.NONE;
		public ContextType context
		{
			get { return mContext; }

			set
			{
				if (mContext == value)
				{
					L.W("trying to set same context. ignore.");
					return;
				}

				mContext = value;
			}
		}

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
				case ContextType.BATTLE:
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
				case ContextType.BATTLE:
					break;
			}
		}

	}
}