using Gem;
using UnityEngine;

namespace Choanji
{
	public class CharacterInputProviderTest : MonoBehaviour
	{
		void Start()
		{
			provider = new CharacterInputProvider(TheGem.g.input) { delegate_ = character };
		}

		public CharacterCtrl character;
		public CharacterInputProvider provider;

	}
}
