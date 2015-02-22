using Gem;
using UnityEngine;

namespace Choanji
{
	public static class TheToast
	{
		private const float MARGIN = 0.2f;
		private const float WIDTH = 0.15f;
		private const float HEIGHT = 60;

		public static Toast Open(string _txt)
		{
			var _toast = PrefabDB.g.toast.Instantiate();
			var _rect = (RectTransform) _toast.transform;
			_rect.SetParent(TheChoanji.g.canvas.transform);
			_rect.pivot = new Vector2(0.5f, 0);
			_rect.anchorMin = new Vector2(0.5f - WIDTH, MARGIN);
			_rect.anchorMax = new Vector2(0.5f + WIDTH, MARGIN);
			_rect.offsetMin = new Vector2(0, 0);
			_rect.offsetMax = new Vector2(0, HEIGHT);
			_toast.txt = _txt;
			return _toast;
		}
	}
}