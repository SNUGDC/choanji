using UnityEngine;
using System.Collections;

public class CharacterInputProvider 
{
	public void Process()
	{
		if (delegate_.CanMove())
		{
			var _dir = InputHelper.GetDirection();
			if (_dir != 0) delegate_.Move(_dir);
		}
	}

	public ICharacterInputDelegate delegate_;
}
