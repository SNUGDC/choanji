using System;
using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace Choanji
{
	using Clips = List<tk2dSpriteAnimationClip>;

	[Serializable]
	public class CharacterSkin
	{
		public CharacterSkin(tk2dSpriteAnimation _anim)
		{
			anim = _anim;
			var _keys = Enum.GetValues(typeof (CharacterAnimKey));
			mClips.Resize(_keys.GetLength(0));
			foreach (var _key in _keys)
				mClips[(int)_key] = _anim.GetClipByName(_key.ToString());
		}

		public tk2dSpriteAnimation anim;

		[SerializeField]
		[HideInInspector]
		private Clips mClips = new Clips();

		public tk2dSpriteAnimationClip this[CharacterAnimKey _key]
		{
			get { return mClips[(int)_key]; }
		}
	}


}