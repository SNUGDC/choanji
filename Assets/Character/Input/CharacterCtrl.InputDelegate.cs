using Gem;
using UnityEngine;

namespace Choanji
{

	public partial class CharacterCtrl : ICharacterInputDelegate
	{
		private const float MOVE_INPUT_DELAY = 0.1f;

		private SewValue<float, ArithmeticFloat> mMoveTimer
			= new SewValue<float, ArithmeticFloat>(MOVE_INPUT_DELAY);

		private void UpdateInput()
		{
			if (!mMoveTimer.isDefault)
				mMoveTimer.Add(Time.deltaTime);
		}

		public void ProcessInput(Direction _dir)
		{
			if (!mMoveTimer.isDefault)
				return;

			D.Assert(curMap != null);

			mMoveTimer.Add(float.Epsilon);
			TryMove(_dir);
		}
	}
}