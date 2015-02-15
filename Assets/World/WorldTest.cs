#if UNITY_EDITOR
using Choanji;
using UnityEngine;

public class WorldTest : MonoBehaviour
{
	public Transform parent;
	public string world;
	
	private void Start()
	{
		TheWorld.parent = parent;
		TheWorld.Read(world);
	}
}

#endif