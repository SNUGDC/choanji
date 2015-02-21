using Gem;
using UnityEngine;

namespace Choanji
{
	public class IntroScene : MonoBehaviour
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
			TheInput.intro.Unreg(mBind);
		}

		private void OnSplashDone()
		{
			TheInput.intro.Reg(mBind);
		}

		private bool OnPressYes()
		{
			TheInput.intro.Unreg(mBind);
			Application.LoadLevel("load");
			return true;
		}
	}
}