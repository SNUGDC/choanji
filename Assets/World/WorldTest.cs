#if UNITY_EDITOR
using Choanji;
using UnityEngine;

public class WorldTest : MonoBehaviour
{
	public string world;

	private void Start()
	{
		TheWorld.bluePrint = WorldBluePrint.Read(world);
	}
}

#endif