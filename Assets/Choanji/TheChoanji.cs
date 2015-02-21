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

				switch (mContext)
				{
					case ContextType.WORLD:
						WorldScene.g.SetActive(false);
						if (Cameras.world)
							Cameras.world.gameObject.SetActive(false);
						break;

					case ContextType.BATTLE:
						Battle.Scene.g.root.SetActive(false);
						break;
				}

				mContext = value;

				switch (value)
				{
					case ContextType.WORLD:
						WorldScene.g.SetActive(true);
						if (Cameras.world)
						{
							Cameras.world.transform.SetParent(TheWorld.parent);
							Cameras.world.gameObject.SetActive(true);
						}
						break;

					case ContextType.BATTLE:
						Battle.Scene.g.root.SetActive(true);
						break;
				}
			}
		}

		void Update()
		{
			var _dt = Time.deltaTime;
			Timer.g.Update(_dt);
			
			switch (context)
			{
				case ContextType.INTRO:
					TheInput.intro.Update();
					break;
				case ContextType.WORLD:
					TheInput.world.Update();
					UI.UI.Update(_dt);
					WorldProgress.Update();
					break;
				case ContextType.BATTLE:
					TheInput.battle.Update();
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