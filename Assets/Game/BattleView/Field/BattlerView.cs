using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	public class BattlerView : MonoBehaviour
	{
		public SpriteRenderer renderer_;

		public void SetBattler(string _key)
		{
			renderer_.sprite = R.BattleUI.Spr.BATTLER_FIELD_ILLUST(_key);
			transform.SetLPosY(renderer_.sprite.rect.height / 200);
		}
	}
}