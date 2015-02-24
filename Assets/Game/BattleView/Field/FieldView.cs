using UnityEngine;

namespace Choanji.Battle
{
	public class FieldView : MonoBehaviour
	{
		public SpriteRenderer bg;
		public SpriteRenderer base_;
		public Transform enemyCenter;

		public EnvType env
		{
			set
			{
				bg.sprite = R.BattleUI.Spr.FIELD_BG(value);
				base_.sprite = R.BattleUI.Spr.FIELD_BASE(value);
			}
		}
	}

}