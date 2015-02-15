using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace Choanji
{
	public class CharacterSkins : MonoBehaviour
	{
		public static CharacterSkins g;

		public enum Key {}

		public const int COUNT = 29;

		[SerializeField]
		[HideInInspector]
		private List<CharacterSkin> mSkins = new List<CharacterSkin>();

		public void Load(Key _key, tk2dSpriteAnimation _anim)
		{
			if (mSkins.Count <= (int)_key - 1)
				mSkins.Resize((int)_key);
			mSkins[(int)_key - 1] = new CharacterSkin(_anim);
		}

		public CharacterSkin this[Key _key]
		{
			get
			{
				return ((int)_key <= mSkins.Count)
					? mSkins[(int)_key - 1] : null;
			}
		}
	}
}