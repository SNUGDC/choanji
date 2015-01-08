using Gem;
using UnityEngine;
using System.Collections;

public partial class Character : ICharacterInputDelegate
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

		mMoveTimer.Add(float.Epsilon);
		transform.localPosition += (Vector3) DirectionHelper.ToVector2(_dir);
	}
}
