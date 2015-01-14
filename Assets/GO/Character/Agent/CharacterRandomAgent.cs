using Gem;
using UnityEngine;

namespace Choanji
{
	public class CharacterRandomAgent : MonoBehaviour
	{
		private ICharacterInputDelegate mDelegate;
		private SewValue<float, ArithmeticFloat> mTimer 
			= new SewValue<float, ArithmeticFloat>(0.6f);

		void Start()
		{
			mDelegate = GetComponent<CharacterCtrl>();
		}

		public void Update()
		{
			if (mTimer.Add(Time.deltaTime))
				mDelegate.ProcessInput(DirectionHelper.Rand());
		}
	}


}