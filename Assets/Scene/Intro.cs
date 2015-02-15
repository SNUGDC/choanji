using Gem.In;
using UnityEngine;

namespace Choanji
{
	public class Intro : MonoBehaviour
	{
		private InputBind mBind;

		void Awake()
		{
			TheChoanji.g.context = ContextType.INTRO;

			mBind = new InputBind(InputCode.Y, new InputHandler()
			{
				down = OnPressYes,
			});
		}

		void OnDestroy()
		{
			InputManager.g.Unreg(mBind);
		}

		private void OnSplashDone()
		{
			InputManager.g.Reg(mBind);
		}

		private bool OnPressYes()
		{
			InputManager.g.Unreg(mBind);
			Application.LoadLevel("disket");
			return true;
		}
	}
}