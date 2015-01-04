using UnityEngine;
using System.Collections;

public partial class Character : ICharacterInputDelegate
{
	private const float INPUT_DELAY = 0.1f;
	private float mInputEllapsed = 0;

	private void UpdateInput()
	{
		mInputEllapsed += Time.deltaTime;
	}

	public bool CanMove()
	{
		return mInputEllapsed > INPUT_DELAY;
	}

	public void Move(Direction _dir)
	{
		if (!CanMove())
		{
			D.Log(2, "Check before call.");
			return;
		}

		if (_dir == 0)
			return;

		mInputEllapsed = 0;
		transform.Translate(DirectionHelper.ToVector2(_dir));
	}
}
