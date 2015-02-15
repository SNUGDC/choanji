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
			TheCharacter.ch = ch;
			ch.renderer_.SetSkin((CharacterSkins.Key)skin);
		}
	}
}

#endif