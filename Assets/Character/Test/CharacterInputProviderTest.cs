using UnityEngine;

namespace Choanji
{
	public class CharacterInputProviderTest : MonoBehaviour
	{
		void Start()
		{
			provider = new CharacterInputProvider(gem.input) { delegate_ = character };
		}

		public Gem.Gem gem;
		public CharacterCtrl character;
		public CharacterInputProvider provider;

	}
}
