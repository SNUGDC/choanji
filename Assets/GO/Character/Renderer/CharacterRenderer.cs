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
			if (_clip == null || _clip.Empty)
			{
				L.D("anim is not exists.");
				return;
			}

			animator.Play(_clip);
		}

		public void LookAt(Direction _dir)
		{
			switch (_dir)
			{
				case Direction.L: Play(CharacterAnimKey.LEFT); break;
				case Direction.R: Play(CharacterAnimKey.RIGHT); break;
				case Direction.U: Play(CharacterAnimKey.UP); break;
				case Direction.D: Play(CharacterAnimKey.DOWN); break;
			}
		}

		public void Walk(Direction _dir)
		{
			switch (_dir)
			{
				case Direction.L: Play(CharacterAnimKey.WALK_LEFT); break;
				case Direction.R: Play(CharacterAnimKey.WALK_RIGHT); break;
				case Direction.U: Play(CharacterAnimKey.WALK_UP); break;
				case Direction.D: Play(CharacterAnimKey.WALK_DOWN); break;
			}
		}
	}
}