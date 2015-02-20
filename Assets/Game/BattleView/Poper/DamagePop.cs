using UnityEngine;
using UnityEngine.UI;

namespace Choanji.Battle
{
	public class DamagePop : MonoBehaviour
	{
		private const float LIFE = 2;
		private const int SIZE_MIN = 16;
		private const int SIZE_MAX = 64;
		private const HP DMG_MIN = (HP)50;
		private const HP DMG_MAX = (HP)300;

		public Text txt;

		public Damage dmg
		{
			set
			{
				txt.color = ElementDB.Get(value.ele).theme;
				txt.text = value.val.ToString();

				var _size = 0;
				if (value > DMG_MAX)
					_size = SIZE_MAX;
				else if (value < DMG_MIN)
					_size = SIZE_MIN;
				else
					_size = (int)(SIZE_MIN + (value.val - DMG_MIN) / (float)(DMG_MAX - DMG_MIN) * (SIZE_MAX - SIZE_MIN));

				txt.fontSize = _size;
			}
		}

		private float mElapsed;

		private float mG;
		private Vector2 mV;

		void Start()
		{
			mG = 10;
			mV = new Vector2(1, 2);
		}

		void Update()
		{
			var _dt = Time.deltaTime;
			mElapsed += _dt;

			if (mElapsed > LIFE)
			{
				Destroy(gameObject);
				return;
			}
			else
			{
				mV.y -= mG * _dt;
				transform.Translate(mV);
			}
		}
	}
}