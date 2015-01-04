using UnityEngine;
using System.Collections;

public class CharacterInputProviderTest : MonoBehaviour
{
	public void Awake()
	{
		provider.delegate_ = character;
	}

	private void Update()
	{
		provider.Process();	
	}

	public Character character;

	public CharacterInputProvider provider = new CharacterInputProvider();

}
