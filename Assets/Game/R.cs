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

		}
	}
}