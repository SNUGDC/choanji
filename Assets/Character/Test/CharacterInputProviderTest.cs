using Gem.In;
using UnityEngine;

public class CharacterInputProviderTest : MonoBehaviour
{
	void Start()
	{
		provider = new CharacterInputProvider(gem.input) {delegate_ = character};
	}

	public Gem.Gem gem;
	public Character character;
	public CharacterInputProvider provider;

}
