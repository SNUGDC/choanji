using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji
{
	public class Toast : MonoBehaviour
	{
		private const float LIFE = 1.5f;
		private const float FADE = 0.5f;

		public Image back;
		public Text uiTxt;

		private float mEllapsed;

		public string txt { set { uiTxt.text = value; } }

		void Update()
		{
			mEllapsed += Time.deltaTime;

			if (mEllapsed > LIFE + FADE)
			{
				Destroy(gameObject);
			}
			else if (mEllapsed > LIFE)
			{
				SetA(1 - (mEllapsed - LIFE) / FADE);
			}
		}

		void SetA(float _val)
		{
			back.SetA(_val);
			uiTxt.SetA(_val);
		}
	}
}