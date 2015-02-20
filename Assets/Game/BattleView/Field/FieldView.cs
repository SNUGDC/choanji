using UnityEngine;

namespace Choanji.Battle
{
	public class FieldView : MonoBehaviour
	{
		public SpriteRenderer base_;
		public SpriteRenderer bg;

		public EnvType env
		{
			set
			{
				base_.sprite = R.BattleUI.Spr.FIELD_BASE(value);
				bg.sprite = R.BattleUI.Spr.FIELD_BG(value);
			}
		}
	}

}