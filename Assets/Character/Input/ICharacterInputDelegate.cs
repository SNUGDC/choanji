using Gem;
using UnityEngine;
using System.Collections;

public interface ICharacterInputDelegate
{
	void ProcessInput(Direction _dir);
}
