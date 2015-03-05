using UnityEngine;

namespace Choanji.Battle
{
	public class FieldView : MonoBehaviour
	{
		public SpriteRenderer bg;
		public SpriteRenderer base_;
		public Animation enemyParent;
		public Transform enemyCenter;

		public void SetEnv(EnvType _env, int _idx)
		{
			bg.sprite = R.BattleUI.Spr.FIELD_BG(_env, _idx);
			base_.sprite = R.BattleUI.Spr.FIELD_BASE(_env, _idx);
		}
	}

}