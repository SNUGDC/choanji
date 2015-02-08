using UnityEngine;

namespace Choanji.Battle
{
	public class HPBar : Bar
	{
		private static readonly Color COLOR_MAX = Color.green;
		private static readonly Color COLOR_MID = Color.yellow;
		private static readonly Color COLOR_MIN = Color.red;

		void Start()
		{
			textCurSize = 28;
		}

		protected override void DoSet(float _val)
		{
			base.DoSet(_val);

			var _prop = _val / max;

			bar.color = (_prop > 0.5f)
				? Color.Lerp(COLOR_MID, COLOR_MAX, 2 * (_prop - 0.5f)) 
				: Color.Lerp(COLOR_MIN, COLOR_MID, 2 * _prop);
		}
	}
}