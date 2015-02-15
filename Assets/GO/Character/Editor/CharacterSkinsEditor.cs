using Gem;
using UnityEditor;
using UnityEngine;

namespace Choanji
{
	[CustomEditor(typeof(CharacterSkins))]
	public class CharacterSkinsEditor : Editor<CharacterSkins>
	{
		private static FullPath Path(int _idx)
		{
			return new FullPath("Assets/R/Character/skin" + _idx + "_anim.prefab");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (GUILayout.Button("build"))
			{
				for (int i = 0; i != CharacterSkins.COUNT; ++i)
				{
					var _anim = TKHelper.AssetAnimation(Path(i + 1));
					if (!_anim) continue;
					target.Load((CharacterSkins.Key)i + 1, _anim);
				}
			}
		}
	}

}