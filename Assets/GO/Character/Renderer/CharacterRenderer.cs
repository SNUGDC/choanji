using Gem;
using UnityEngine;

namespace Choanji
{
	public class CharacterRenderer : MonoBehaviour
	{
		public tk2dSprite sprite;
		public tk2dSpriteAnimator animator;

		private CharacterSkins.Key mSkinKey;
		private CharacterSkin mSkin;

		private Direction mDir = Direction.D;

		private float mStopElapsed = 0;

		private void Update()
		{
			if (animator.Playing)
			{
				mStopElapsed = 0;
			}
			else if (mStopElapsed >= 0)
			{
				mStopElapsed += Time.deltaTime;
				if (mStopElapsed > 0.05f)
				{ 
					mStopElapsed = -1;
					LookAt(mDir);
				}
			}
		}

		public CharacterSkins.Key GetSkinKey()
		{
			return mSkinKey;
		}

		public void SetSkin(CharacterSkins.Key _key)
		{
			mSkinKey = _key;
			mSkin = CharacterSkins.g[_key];
			LookAt(Direction.D);
		}

		private void Play(CharacterAnimList _key)
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
			mDir = _dir;
			switch (_dir)
			{
				case Direction.L: Play(CharacterAnimList.LEFT); break;
				case Direction.R: Play(CharacterAnimList.RIGHT); break;
				case Direction.U: Play(CharacterAnimList.UP); break;
				case Direction.D: Play(CharacterAnimList.DOWN); break;
			}
		}

		public void Walk(Direction _dir)
		{
			mDir = _dir;
			switch (_dir)
			{
				case Direction.L: Play(CharacterAnimList.WALK_LEFT); break;
				case Direction.R: Play(CharacterAnimList.WALK_RIGHT); break;
				case Direction.U: Play(CharacterAnimList.WALK_UP); break;
				case Direction.D: Play(CharacterAnimList.WALK_DOWN); break;
			}
		}
	}
}