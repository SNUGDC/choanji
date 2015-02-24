using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji.Battle
{
	public class APBar : Bar
	{
		public Image highlightImg;

		public bool isHighlighted { get { return highlight != 0; } }

		private AP mHighlight;
		public AP highlight
		{
			private get { return mHighlight; }
			set
			{
				if (mHighlight == value)
					return;
				mHighlight = value;
				highlightImg.gameObject.SetActive(highlight != 0);
				ResizeHighlight();
			}
		}

		private TrianglarFloat mHighlightPeriod = new TrianglarFloat(0.3f);

		void Start()
		{
			textCurSize = 15;
		}

		protected override void Update()
		{
			base.Update();

			if (isHighlighted)
			{
				mHighlightPeriod.Add(Time.deltaTime);
				highlightImg.SetA(mHighlightPeriod);
			}
		}

		protected override void Resize()
		{
			base.Resize();
			ResizeHighlight();
		}

		public void ResizeHighlight()
		{
			if (isHighlighted)
			{
				var _highlightVal = Mathf.Max(0, val - (int)highlight);
				highlightImg.rectTransform.SetMinX(_highlightVal / max * width);
				highlightImg.rectTransform.SetMaxX(bar.rectTransform.MaxX());
			}
		}
	}
}