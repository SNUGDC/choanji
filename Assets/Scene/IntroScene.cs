using UnityEngine;

namespace Choanji
{
	public class IntroScene : MonoBehaviour
	{
		private bool mIsSplashDone;
		private bool mIsLevelLoaded;

		void Awake()
		{
			TheChoanji.g.context = ContextType.INTRO;
		}

		void Update()
		{
			if (!mIsSplashDone)
				return;

			if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
				OnPressYes();
		}

		private void OnSplashDone()
		{
			mIsSplashDone = true;
		}

		private void OnPressYes()
		{
			if (mIsLevelLoaded) return;
			mIsLevelLoaded = true;
			Application.LoadLevel("load");
		}
	}
}