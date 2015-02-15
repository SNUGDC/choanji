using Gem;
using UnityEngine;

namespace Choanji
{
	public class CharacterRenderer : MonoBehaviour
	{
		public tk2dSprite sprite;
		public tk2dSpriteAnimator animator;

		private CharacterSkin mSkin;

		public void Set(CharacterSkins.Key _key)
		{
			mSkin = CharacterSkins.g[_key];
			Play(CharacterAnimKey.DOWN);
		}

		public void Play(CharacterAnimKey _key)
		{
			if (mSkin == null)
			{
				L.W("skin is not exists.");
				return;
			}

			var _clip = mSkin[_key];
			if (_clip == null)
			{
				L.W("anim is not exists.");
				return;
			}

			animator.Play(_clip);
		}
	}
}