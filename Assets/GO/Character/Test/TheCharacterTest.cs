#if UNITY_EDITOR

using UnityEngine;

namespace Choanji
{

	public class TheCharacterTest : MonoBehaviour
	{
		public Character ch;
		public int skin;

		void Start()
		{
			TheCharacter.g = ch;
			ch.renderer_.Set((CharacterSkins.Key)skin);
		}
	}
}

#endif