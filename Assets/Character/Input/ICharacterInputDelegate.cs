using UnityEngine;
using System.Collections;

public interface ICharacterInputDelegate
{
	bool CanMove();

	void Move(Direction _dir);
}
