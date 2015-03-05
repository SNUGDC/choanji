using Gem;
using UnityEngine;

namespace Choanji
{

	public partial class CharacterCtrl : ICharacterInputDelegate
	{
		private const float MOVE_INPUT_DELAY = 0.1f;

		private SawFloat mMoveTimer = new SawFloat(MOVE_INPUT_DELAY);

		private void UpdateInput()
		{
			if (!mMoveTimer.isDefault)
				mMoveTimer.Add(Time.deltaTime);
		}

		public void ProcessInputYes()
		{
			if (!isInspecting)
				Inspect();
		}

		public void ProcessInput(Direction _dir)
		{
			if (!mMoveTimer.isDefault)
				return;

			mMoveTimer.Add(float.Epsilon);

			if (!TryMove(_dir))
				TryTurn(_dir);
		}
	}
}