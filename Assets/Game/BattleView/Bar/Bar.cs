﻿using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji
{
	public class Bar : MonoBehaviour
	{
		public Image bar;
		public Text text;

		public int max { protected get; set; }

		protected float width { get { return ((RectTransform) transform).W(); } }

		protected int textCurSize { private get; set; }

		public void Set(float _val)
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

			DoSet(_val);
		}

		protected virtual void DoSet(float _val)
		{
			if (text)
				text.text = BuildText(_val);

			var _prop = _val / max;
			bar.rectTransform.SetMaxX((_prop - 1) * width);
		}

		private string BuildText(float _val)
		{
			return "<size=" + textCurSize + ">" + (int) (_val + 0.01f) + "</size> / " + max;
		}
	}
}