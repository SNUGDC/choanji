#if UNITY_EDITOR
using Gem;
using UnityEditor;

namespace Choanji.UI
{
	public class UITest 
	{
		[MenuItem("Test/UI/Toggle Top Menu")]
		public static void ToggleTopMenu()
		{
			if (App.playing)
				UI.ToggleTopMenu();
		}
	}
}
#endif