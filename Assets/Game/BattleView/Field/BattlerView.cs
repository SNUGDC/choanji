using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	public class BattlerView : MonoBehaviour
	{
		public SpriteRenderer renderer_;

		public void SetBattler(string _key)
		{
			var _spr = R.BattleUI.Spr.BATTLER_FIELD_ILLUST(_key);
			if (_spr)
			{
				renderer_.sprite = _spr;
				transform.SetLPosY(renderer_.sprite.rect.height / 200);
			}
		}
	}
}