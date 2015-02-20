using Choanji.Battle;
using Gem;
using UnityEngine;

namespace Choanji.R
{
	public static class Spr
	{
		public static Sprite ELE_S(ElementID _id)
		{
			return ELE_S(ElementDB.Get(_id).key);
		}

		public static Sprite ELE_S(string _key)
		{
			return Resources.Load<Sprite>("Element/ICON_" + _key);
		}

	}

	public static class Snd
	{
		private static AudioClip Clip(string _name)
		{
			var _clip = Resources.Load<AudioClip>(_name);
			if (!_clip) L.E("clip not found " + _name);
			return _clip;
		}

		public static AudioClip BGM(string _name)
		{
			return Clip(_name);
		}

		public static AudioClip SFX(string _name)
		{
			return Clip(_name);
		}
	}

	namespace BattleUI
	{
		public static class Spr
		{
			public static Sprite CARD_ACTIVE_TYPE(ActiveType _type)
			{
				return Resources.Load<Sprite>("BattleUI/Active/ICON_" + _type);
			}

			public static Sprite CARD_ILLUST_S(string _key)
			{
				var _tex = TextureCache.Load(new FullPath("Resources/Card/" + _key + "_ILLU_S.png"));
				return _tex.CreateSpite();
			}

			public static Sprite FIELD_BG(EnvType _env)
			{
				return Resources.Load<Sprite>("Battlebacks/BG_" + _env);
			}

			public static Sprite FIELD_BASE(EnvType _env)
			{
				return Resources.Load<Sprite>("Battlebacks/BASE_" + _env);
			}

			public static Sprite BATTLER_FIELD_ILLUST(string _key)
			{
				return TextureCache.Load(new FullPath("Resources/Battler/" + _key + ".png")).CreateSpite();
			}
		}
	}
}