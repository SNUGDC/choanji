using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji
{
	public class Bar : MonoBehaviour
	{
		public Image bar;
		public Text text;

		public int max { protected get; set; }
		protected float val { get; private set; }

		protected float width { get { return ((RectTransform) transform).W(); } }

		protected int textCurSize { private get; set; }

		private float? mTarget;

		protected virtual void Update()
		{
			if (!mTarget.HasValue)
				return;

			if (Mathf.Abs(val - mTarget.Value) > 1)
			{
				DoSet(Mathf.Lerp(val, mTarget.Value, 2 * Time.deltaTime));
			}
			else
			{
				DoSet(mTarget.Value);
				mTarget = null;
			}
		}

		public void Set(float _val, bool _lerp)
		{
			if (!_lerp)
			{
				mTarget = null;
				DoSet(_val);
			}
			else
			{
				mTarget = _val;
			}
		}

		private void DoSet(float _val)
		{
			if (max == 0)
			{
				L.E("trying to set value without set max.");
				return;
			}

			if (_val > max)
			{
				L.W("val is bigger than max. trim.");
				_val = max;
			}

			val = _val;

			Resize();
		}

		public void Full(bool _lerp = false)
		{
			Set(max, _lerp);
		}

		protected virtual void Resize()
		{
			if (text)
				text.text = BuildText(val);

			var _prop = val / max;
			bar.rectTransform.SetMaxX((_prop - 1) * width);
		}

		private string BuildText(float _val)
		{
			return "<size=" + textCurSize + ">" + (int) (_val + 0.01f) + "</size> / " + max;
		}
	}
}