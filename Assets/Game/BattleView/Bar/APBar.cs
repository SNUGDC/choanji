using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji.Battle
{
	public class APBar : Bar
	{
		public Image highlightImg;

		public bool isHighlighted { get { return highlight != 0; } }

		private int mHighlight;
		public int highlight
		{
			private get { return mHighlight; }
			set
			{
				if (mHighlight == value)
					return;
				mHighlight = value;
				highlightImg.gameObject.SetActive(highlight != 0);
			}
		}

		private TrianglarFloat mHighlightPeriod = new TrianglarFloat(0.3f);

		void Start()
		{
			textCurSize = 15;
		}

		void Update()
		{
			if (isHighlighted)
			{
				mHighlightPeriod.Add(Time.deltaTime);
				highlightImg.SetA(mHighlightPeriod);
			}
		}

		protected override void DoSet(float _val)
		{
			base.DoSet(_val);

			if (isHighlighted)
			{
				var _highlightVal = Mathf.Max(0, _val - highlight);
				highlightImg.rectTransform.SetMinX(_highlightVal/max * width);
				highlightImg.rectTransform.SetMaxX(bar.rectTransform.MaxX());
			}
		}
	}
}